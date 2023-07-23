using System.Security.Cryptography;
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

}
