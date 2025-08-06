using System.Text;
using TUnit.Assertions.AssertConditions.Throws;

namespace CommonNet.Extensions.Tests;

public class EnumerableExtensionsTests
{
    [Test]
    public async Task ForEach_ArgumentsConstraintsTests_ShouldSucceed()
    {
        var data = Array.Empty<object>();

        await Assert.That(() => data.ForEach(null!))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
#pragma warning disable TUnit0001 // Invalid Data for Tests
    [IntArrayGenerator(100, AllowEmpty: false)]
#pragma warning restore TUnit0001 // Invalid Data for Tests
    [ArgumentDisplayFormatter<EnumerableFormatter>]
    public async Task ForEach_ShouldEnumerateAllValues_PropertyTest(int[] data)
    {
        StringBuilder sb = new();
        data.ForEach(v => sb.Append(v));
        await Assert.That(() => sb.ToString())
            .IsEqualTo(string.Join(string.Empty, data.Select(v => v.ToString())));
    }

    private class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    [Test]
    public async Task Median_WithDoubleSelector_ReturnsCorrectMedian()
    {
        var people = new List<Person>
        {
            new() { Name = "Alice", Age = 25 },
            new() { Name = "Bob", Age = 30 },
            new() { Name = "Charlie", Age = 40 }
        };
        await Assert.That(people.Median(person => person.Age))
            .IsEqualTo(30.0);

    }

    [Test]
    public async Task Median_WithDoubleSequence_ReturnsCorrectMedian()
    {
        // odd case
        List<double> numbers = [1.0, 2.0, 3.0, 4.0, 5.0];
        await Assert.That(numbers.Median())
            .IsEqualTo(3.0);

        // even case
        numbers = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
        await Assert.That(numbers.Median())
            .IsEqualTo(3.5);
    }

    [Test]
    public async Task Median_WithIntSequence_ReturnsCorrectMedian()
    {
        // odd case
        List<int> numbers = [1, 2, 3, 4, 5];
        var median = numbers.Median();
        await Assert.That(median)
            .IsEqualTo(3);

        // even case
        numbers = [1, 2, 3, 4, 5, 6];
        median = numbers.Median();
        await Assert.That(median)
            .IsEqualTo(3.5);
    }

    [Test]
    public async Task Median_EmptySequence_ThrowsArgumentException()
    {
        var emptyList = new List<int>();
        await Assert.That(() => emptyList.Median())
            .ThrowsExactly<ArgumentException>();
    }

    [Test]
    public async Task Median_NullSelector_ThrowsArgumentNullException()
    {
        var people = new List<Person> { new() { Name = "Alice", Age = 25 } };
        await Assert.That(() => people.Median(null!))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task Median_NullSequence_ThrowsArgumentNullException()
    {
        IEnumerable<double> nullSequence = null!;
        await Assert.That(() => nullSequence.Median())
            .ThrowsExactly<ArgumentNullException>();
    }
}
