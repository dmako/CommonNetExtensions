using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class SpanExtensions
{
    [Fact]
    public void ByteArray_AsReadOnlySpan()
    {
        const byte[]? nullArr = null;
        Action action = () => nullArr!.AsReadOnlySpan();
        action.Should().ThrowExactly<ArgumentNullException>();

        var arr = new byte[] { 0x00 };
        arr.Length.Should().Be(1);
    }

    [Fact]
    public void ByteArray_XorBasicTest()
    {
        const byte[]? nullArr = null;
        var arr = new byte[] { 0x00 };
        var arr2 = new byte[] { 0x01, 0x02 };

        Action action = () => nullArr!.Xor(0, 1, arr);
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => arr.Xor(0, 1, null);
        action.Should().ThrowExactly<ArgumentException>();
        action = () => arr.Xor(0, 1, Array.Empty<byte>());
        action.Should().ThrowExactly<ArgumentException>();
        action = () => arr.Xor(1, 1, arr);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        action = () => arr2.Xor(1, 2, arr);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [FsCheck.Xunit.Property(MaxTest = 100, Arbitrary = new[] { typeof(NonEmptyArrayArbitrary) }, DisplayName = nameof(ByteArray_PropertyXorTest), QuietOnSuccess = true)]
    public void ByteArray_PropertyXorTest(byte[] data, byte[] key)
    {
        if (key.All(x => x != 0))
        {
            // xor by 0 does not change the input and the test expectactions might not be met
            var local = new byte[data.Length];
            Array.Copy(data, local, data.Length);
            local.Xor(0, local.Length, key);
            data.Should().NotBeEquivalentTo(local);
            local.Xor(0, local.Length, key);
            data.Should().BeEquivalentTo(local);
        }
    }
}
