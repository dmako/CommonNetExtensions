using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class SpanExtensionsTests
{
    [Fact]
    public void ByteArray_AsReadOnlySpan_ArgumentsChecksShouldBehaveAsExpected_And_ShouldProduceCorrectSpan()
    {
        const byte[]? nullArr = null;
        Action action = () => _ = nullArr!.AsReadOnlySpan();
        action.Should().ThrowExactly<ArgumentNullException>();

        var arr = new byte[] { 0x00 };
        arr.Should().ContainSingle();
    }

    [Fact]
    public void ByteArray_Xor_ArgumentsChecksShouldBehaveAsExpected()
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

    [FsCheck.Xunit.Property(MaxTest = 100, Arbitrary = new[] { typeof(NonEmptyArrayArbitrary) }, DisplayName = nameof(ByteArray_XorXor_ShouldProduceSameOutputAsInput_PropertyTest), QuietOnSuccess = true)]
    public void ByteArray_XorXor_ShouldProduceSameOutputAsInput_PropertyTest(byte[] data, byte[] key)
    {
        var local = new byte[data.Length];
        Array.Copy(data, local, data.Length);
        local.Xor(0, local.Length, key);

        if (key.All(x => x != 0))
        {
            // xor by 0 does not change the input and the test expectactions might not be met
            data.Should().NotBeEquivalentTo(local);
        }
        local.Xor(0, local.Length, key);
        data.Should().BeEquivalentTo(local);
    }

    [Fact]
    public void AllIndexesOf_ShouldReturnCorrectIndexes_WhenPatternIsFound()
    {
        var source = new ReadOnlySpan<int>(new int[] { 1, 2, 3, 4, 5, 2, 3, 4, 2, 3 });
        var pattern = new ReadOnlySpan<int>(new int[] { 2, 3 });
        var expectedIndexes = new List<int> { 1, 5, 8 };

        var result = source.AllIndexesOf(pattern);
        result.Should().BeEquivalentTo(expectedIndexes);
    }

    [Fact]
    public void AllIndexesOf_ShouldReturnEmptyEnumerable_WhenPatternIsNotFound()
    {
        var source = new ReadOnlySpan<int>(new int[] { 1, 2, 3, 4, 5 });
        var pattern = new ReadOnlySpan<int>(new int[] { 6, 7 });

        var result = source.AllIndexesOf(pattern);
        result.Should().BeEmpty();
    }

    [Fact]
    public void AllIndexesOf_ShouldReturnAllIndexes_WhenPatternIsSingleElement()
    {
        var source = new ReadOnlySpan<int>(new int[] { 1, 2, 3, 2, 3, 4, 5 });
        var pattern = new ReadOnlySpan<int>(new int[] { 2 });
        var expectedIndexes = new List<int> { 1, 3 };

        var result = source.AllIndexesOf(pattern);
        result.Should().BeEquivalentTo(expectedIndexes);
    }

    [Fact]
    public void AllIndexesOf_ShouldThrow_WhenPatternIsEmpty()
    {
        var source = new ReadOnlyMemory<int>(new int[] { 1, 2, 3 });

        var func = () => source.Span.AllIndexesOf(ReadOnlySpan<int>.Empty);
        func.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AllIndexesOf_ShouldReturnEmptyEnumerable_WhenSourceIsEmptyAndPatternIsNotEmpty()
    {
        var source = ReadOnlySpan<int>.Empty;
        var pattern = new ReadOnlySpan<int>(new int[] { 1, 2 });

        var result = source.AllIndexesOf(pattern);
        result.Should().BeEmpty();
    }
}
