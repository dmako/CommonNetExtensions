using System.Data;
using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void ForEach_ArgumentsConstraintsTests_ShouldSucceed()
    {
        var data = Array.Empty<object>();

        Action action = () => ((object[])null!).ForEach(o => { });
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => data.ForEach(null!);
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Property(MaxTest = 100, DisplayName = nameof(ForEach_ShouldEnumerateAllValues_PropertyTest), QuietOnSuccess = true)]
    public void ForEach_ShouldEnumerateAllValues_PropertyTest(int[] data)
    {
        var sum = 0;
        data.ForEach(v => sum += v);
        sum.Should().Be(data.Sum());
    }

    private class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    [Fact]
    public void Median_WithDoubleSelector_ReturnsCorrectMedian()
    {
        var people = new List<Person>
        {
            new Person { Name = "Alice", Age = 25 },
            new Person { Name = "Bob", Age = 30 },
            new Person { Name = "Charlie", Age = 40 }
        };
        var medianAge = people.Median(person => person.Age);
        medianAge.Should().Be(30.0);
    }

    [Fact]
    public void Median_WithDoubleSequence_ReturnsCorrectMedian()
    {
        // odd case
        var numbers = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 };
        var median = numbers.Median();
        median.Should().Be(3.0);

        // even case
        numbers = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
        median = numbers.Median();
        median.Should().Be(3.5);
    }

    [Fact]
    public void Median_WithIntSequence_ReturnsCorrectMedian()
    {
        // odd case
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var median = numbers.Median();
        median.Should().Be(3);

        // even case
        numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        median = numbers.Median();
        median.Should().Be(3.5);
    }

    [Fact]
    public void Median_EmptySequence_ThrowsArgumentException()
    {
        var emptyList = new List<int>();
        var act = () => emptyList.Median();
        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void Median_NullSelector_ThrowsArgumentNullException()
    {
        var people = new List<Person> { new Person { Name = "Alice", Age = 25 } };
        var act = () => people.Median(null!);
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Median_NullSequence_ThrowsArgumentNullException()
    {
        IEnumerable<double> nullSequence = null!;
        var act = () => nullSequence.Median();
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}
