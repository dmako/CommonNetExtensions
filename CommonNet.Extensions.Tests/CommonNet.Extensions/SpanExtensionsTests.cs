using TUnit.Assertions.AssertConditions.Throws;

namespace CommonNet.Extensions.Tests;

public class SpanExtensionsTests
{
    [Test]
    public async Task ByteArray_AsReadOnlySpan_ArgumentsChecksShouldBehaveAsExpected_And_ShouldProduceCorrectSpan()
    {
        const byte[]? nullArr = null;

        await Assert.That(() => _ = nullArr!.AsReadOnlySpan())
            .ThrowsExactly<ArgumentNullException>();

        await Assert.That(() => new byte[] { 0x00 }.AsReadOnlySpan().Length)
            .IsEqualTo(1);
    }

    [Test]
    public async Task ByteArray_Xor_ArgumentsChecksShouldBehaveAsExpected()
    {
        const byte[]? nullArr = null;
        var arr = new byte[] { 0x00 };
        var arr2 = new byte[] { 0x01, 0x02 };

        await Assert.That(() => nullArr!.Xor(0, 1, arr))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => arr.Xor(0, 1, null))
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => arr.Xor(0, 1, []))
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => arr2.Xor(1, 2, arr))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    [NonEmptyByteArrayPairGenerator(ItemsCount: 100)]
    public async Task ByteArray_XorXor_ShouldProduceSameOutputAsInput_PropertyTest(ByteArrayPair values)
    {
        var (data, key) = values;
        var local = new byte[data.Length];
        Array.Copy(data, local, data.Length);
        local.Xor(0, local.Length, key);

        if (key.All(x => x != 0))
        {
            // xor by 0 does not change the input and the test expectations might not be met
            await Assert.That(data)
                .IsNotEquivalentTo(local);
        }
        local.Xor(0, local.Length, key);
        await Assert.That(data)
            .IsEquivalentTo(local);
    }

    [Test]
    public async Task AllIndexesOf_ShouldReturnCorrectIndexes_WhenPatternIsFound()
    {
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5, 2, 3, 4, 2, 3]);
        var pattern = new ReadOnlySpan<int>([2, 3]);
        var expectedIndexes = new List<int> { 1, 5, 8 };

        var result = source.AllIndexesOf(pattern);
        await Assert.That(result)
            .IsEquivalentTo(expectedIndexes);
    }

    [Test]
    public async Task AllIndexesOf_ShouldReturnEmptyEnumerable_WhenPatternIsNotFound()
    {
        var source = new int[] { 1, 2, 3, 4, 5 };
        var pattern = new int[] { 6, 7 };

        await Assert.That(() => source.AsReadOnlySpan().AllIndexesOf(pattern.AsReadOnlySpan()))
            .IsEmpty();
    }

    [Test]
    public async Task AllIndexesOf_ShouldReturnAllIndexes_WhenPatternIsSingleElement()
    {
        var source = new int[] { 1, 2, 3, 2, 3, 4, 5 };
        var pattern = new int[] { 2 };
        var expectedIndexes = new List<int> { 1, 3 };

        await Assert.That(() => source.AsReadOnlySpan().AllIndexesOf(pattern.AsReadOnlySpan()))
            .IsEquivalentTo(expectedIndexes);
    }

    [Test]
    public async Task AllIndexesOf_ShouldThrow_WhenPatternIsEmpty()
    {
        var source = new int[] { 1, 2, 3 };

        await Assert.That(() => source.AsReadOnlySpan().AllIndexesOf([]))
            .ThrowsExactly<ArgumentException>();
    }

    [Test]
    public async Task AllIndexesOf_ShouldReturnEmptyEnumerable_WhenSourceIsEmptyAndPatternIsNotEmpty()
    {
        var pattern = new int[] { 1, 2 };
        await Assert.That(() => ReadOnlySpan<int>.Empty.AllIndexesOf(pattern.AsReadOnlySpan()))
            .IsEmpty();
    }
}
