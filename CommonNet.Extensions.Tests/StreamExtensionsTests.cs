using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Moq;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class StreamExtensionsTests
{
    [Fact]
    public async Task ReadChunksAsync_ShouldEnumerateStreamContentInChunks()
    {

        var data = new byte[64];
        var random = new System.Random();
        random.NextBytes(data);

        var bufferSize = 16;
        var expectedChunks = new List<ReadOnlyMemory<byte>>
        {
            new ReadOnlyMemory<byte>(data, 0, bufferSize),
            new ReadOnlyMemory<byte>(data, bufferSize, bufferSize),
            new ReadOnlyMemory<byte>(data, bufferSize * 2, bufferSize),
            new ReadOnlyMemory<byte>(data, bufferSize * 3, bufferSize),
        };

        var stream = new MemoryStream(data);
        var result = new List<ReadOnlyMemory<byte>>();
        await foreach (var chunk in stream.ReadChunksAsync(bufferSize))
        {
            result.Add(chunk.ToArray());
        }

        result.Should().HaveCount(expectedChunks.Count);
        for (var i = 0; i < expectedChunks.Count; i++)
        {
            result[i].ToArray().Should().BeEquivalentTo(expectedChunks[i].ToArray());
        }
    }

    [Fact]
    public async Task ReadChunksAsync_ShouldReturnEmptyResult_WhenStreamIsEmpty()
    {
        var mockStream = new Mock<Stream>();
        mockStream.Setup(s => s.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0)); // EOF

        var bufferSize = 16;
        var result = new List<ReadOnlyMemory<byte>>();
        await foreach (var chunk in mockStream.Object.ReadChunksAsync(bufferSize))
        {
            result.Add(chunk);
        }

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ReadChunksAsync_ShouldUseDefaultBufferSize_WhenBufferSizeIsNotProvided()
    {
        var mockStream = new Mock<Stream>();
        var dataHeader = new byte[] { 0xde, 0xad, 0xbe, 0xef };

#if NET6_0_OR_GREATER
        mockStream.Setup(s => s.ReadAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns<Memory<byte>, CancellationToken>((buffer, cancellationToken) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return ValueTask.FromCanceled<int>(cancellationToken);
                }
                dataHeader.AsReadOnlySpan().CopyTo(buffer.Span);
                return ValueTask.FromResult(buffer.Length);
            });
#else
        mockStream.Setup(s => s.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns<byte[], int, int, CancellationToken>((buffer, offset, count, cancellationToken) =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled<int>(cancellationToken);
                }

                Array.Copy(dataHeader, offset, buffer, 0, dataHeader.Length);
                return Task.FromResult(count);
            });
#endif

        var result = new List<ReadOnlyMemory<byte>>();
        var cts = new CancellationTokenSource();
        try
        {
            await foreach (var chunk in mockStream.Object.ReadChunksAsync(cancellationToken: cts.Token))
            {
                result.Add(chunk);
                cts.Cancel();
            }
        }
        catch (OperationCanceledException)
        {
        }

        var defaultBufferSize = 0x10_000;
        result.Should().ContainSingle();
        result[0].Length.Should().Be(defaultBufferSize);
    }

    [Fact]
    public async Task CopyToWithProgressAndHashAsync_ShouldCopyStreamAndCalculateHash()
    {
        var sourceData = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 };
        var destinationData = new byte[5];

        using var sourceStream = new MemoryStream(sourceData);
        using var destinationStream = new MemoryStream(destinationData);

        var bufferSize = 2;

        long totalBytesRead = 0;
        Action<long> updateTotalBytesRead = bytesRead => totalBytesRead = bytesRead;

        var hashValue = await sourceStream.CopyToWithProgressAndHashAsync<SHA256>(destinationStream, updateTotalBytesRead, bufferSize);

        totalBytesRead.Should().Be(sourceData.Length);
        destinationData.Should().BeEquivalentTo(sourceData);
        hashValue.Should().Be("6C450E037E79B76F231A71A22FF40403F7D9B74B15E014E52FE1156D3666C3E6");
    }

    [Fact]
    public async Task CopyToWithProgressAndHashAsync_ShouldReturnEmptyHash_WhenSourceStreamIsEmpty()
    {
        var emptyData = Array.Empty<byte>();
        using var sourceStream = new MemoryStream(emptyData);
        using var destinationStream = new MemoryStream();

        var bufferSize = 2;

        long totalBytesRead = 0;
        Action<long> updateTotalBytesRead = bytesRead => totalBytesRead = bytesRead;

        var hashValue = await sourceStream.CopyToWithProgressAndHashAsync<SHA256>(destinationStream, updateTotalBytesRead, bufferSize);

        totalBytesRead.Should().Be(0);
        hashValue.Should().Be("E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855");
    }

    public static readonly IList<object[]> ComputeHashData =
    new List<object[]>
    {
        new object[]
        {
            "Hello, hash me!!!",
            "39951640B6DA39AB3B97DE3F5B48D79B",
            "AFE6B43272DD26D18322D01A1FD5582E2C1187C2",
            "C5BA80CBB4A5A5CE07EE8F42325F28C8AF1EE6DAF0DBEC1B1F46C025CDC44477",
            "F38E5A0503DF8EC4AE5D2BF2283ECD74CDE2C6993533D0D91E359DAE1B5A22533B129F260D9B3D26EFC966AAA5CA620F734E85A66490376E5794F24858B55788"
        },
        new object[]
        {
            "The quick brown fox jumps over the lazy dog. 1234567890",
            "BFB85E401A205CDE01D17164BD3DE689",
            "3A6CD59F35D9C8A5E202D904CBCEDD8175CA1632",
            "A4DF915F4220CAF6152E8B5ADAEC19D015AAF2B540F8D565B19885A8EF186A36",
            "FA8418498F96F374BA634FFAC226724BD7E950046E63161C8C149FC3C2E26BD8D7FF8D6C4588A48551F86CE815532004F01F8C4CDDEC3EEB95B4982155CA5F9F"
        }
    };

    [Theory]
    [MemberData(nameof(ComputeHashData))]
    public async Task ComputeHashAsync_ShouldComputeCorrectHashes_ForGivenData(string inputDataString, string expectedMd5, string expectedSha1, string expectedSha256, string expectedSha512)
    {
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(inputDataString));

        var md5 = await inputStream.ComputeHashAsync<MD5>();
        md5.Should().Be(expectedMd5);

        inputStream.Position = 0;
        var sha1 = await inputStream.ComputeHashAsync<SHA1>();
        sha1.Should().Be(expectedSha1);

        inputStream.Position = 0;
        var sha256 = await inputStream.ComputeHashAsync<SHA256>();
        sha256.Should().Be(expectedSha256);

        inputStream.Position = 0;
        var sha512 = await inputStream.ComputeHashAsync<SHA512>();
        sha512.Should().Be(expectedSha512);
    }

}
