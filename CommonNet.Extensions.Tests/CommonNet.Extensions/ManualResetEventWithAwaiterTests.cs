using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class ManualResetEventWithAwaiterTests
{
    [Fact]
    public void ManualResetEventWithAwaiter_ShouldWaitForAwaiterAndReset()
    {
        var count = 0;
        var asyncFunc = async () => { await Task.Delay(TimeSpan.FromMilliseconds(1)); count++; };
        var mre = new ManualResetEventWithAwaiter();

        var awaiter = asyncFunc().GetAwaiter();
        mre.Wait(awaiter);

        awaiter.IsCompleted.Should().BeTrue();
        mre.IsSet.Should().BeFalse();
        count.Should().Be(1);

        awaiter = asyncFunc().GetAwaiter();
        mre.Wait(awaiter);

        awaiter.IsCompleted.Should().BeTrue();
        mre.IsSet.Should().BeFalse();
        count.Should().Be(2);
    }
}
