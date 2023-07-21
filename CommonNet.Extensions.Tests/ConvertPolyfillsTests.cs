#if NET48

using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class ConvertPolyfillsTests
{
    [Fact]
    public void ToHexString_ShouldConvertByteArray_ToHex16String()
    {
        var byteArray = new byte[] { 0x12, 0xAB, 0xCD, 0xEF };
        var result = ConvertPolyfills.ToHexString(byteArray);
        result.Should().Be("12ABCDEF");
    }

    [Fact]
    public void ToHexString_WithOffsetAndLength_ShouldConvertPartialByteArray_ToHex16String()
    {
        var byteArray = new byte[] { 0x12, 0xAB, 0xCD, 0xEF };
        var result = ConvertPolyfills.ToHexString(byteArray, 1, 2);
        result.Should().Be("ABCD");
    }

    [Fact]
    public void ToHexString_WithEmptyByteArray_ShouldReturnEmptyString()
    {
        var byteArray = Array.Empty<byte>();
        var result = ConvertPolyfills.ToHexString(byteArray);
        result.Should().BeEmpty();
    }

    [Fact]
    public void ToHexString_WithEmptySpan_ShouldReturnEmptyString()
    {
        var emptySpan = ReadOnlySpan<byte>.Empty;
        var result = ConvertPolyfills.ToHexString(emptySpan);
        result.Should().BeEmpty();
    }
}

#endif
