using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class EnumerableExtensions
{
    [Fact]
    public void Enumerable_ForEachBasicTest()
    {
        var data = Array.Empty<object>();

        Action action = () => ((object[])null!).ForEach(o => { });
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => data.ForEach(null!);
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Property(MaxTest = 100, DisplayName = nameof(Enumerable_PropertyForEachTest), QuietOnSuccess = true)]
    public void Enumerable_PropertyForEachTest(int[] data)
    {
        var sum = 0;
        data.ForEach(v => sum += v);
        sum.Should().Be(data.Sum());
    }
}
