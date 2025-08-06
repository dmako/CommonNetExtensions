#if NET48

namespace CommonNet.Extensions.Tests;

public class ConvertPolyfillsTests
{
    [Test]
    public async Task ToHexString_ShouldConvertByteArray_ToHex16String()
    {
        var byteArray = new byte[] { 0x12, 0xAB, 0xCD, 0xEF };
        var result = ConvertPolyfills.ToHexString(byteArray);
        await Assert.That(result)
            .IsEqualTo("12ABCDEF");
    }

    [Test]
    public async Task ToHexString_WithOffsetAndLength_ShouldConvertPartialByteArray_ToHex16String()
    {
        var byteArray = new byte[] { 0x12, 0xAB, 0xCD, 0xEF };
        var result = ConvertPolyfills.ToHexString(byteArray, 1, 2);
        await Assert.That(result)
            .IsEqualTo("ABCD");
    }

    [Test]
    public async Task ToHexString_WithEmptyByteArray_ShouldReturnEmptyString()
    {
        var byteArray = Array.Empty<byte>();
        var result = ConvertPolyfills.ToHexString(byteArray);
        await Assert.That(result)
            .IsEmpty();
    }

    [Test]
    public async Task ToHexString_WithEmptySpan_ShouldReturnEmptyString()
    {
        var emptySpan = ReadOnlySpan<byte>.Empty;
        var result = ConvertPolyfills.ToHexString(emptySpan);
        await Assert.That(result)
            .IsEmpty();
    }
}

#endif
