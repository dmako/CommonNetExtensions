using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class WaitHandleExtensionsTests
{
    [Fact]
    public async Task AsTask_ShouldComplete_WhenWaitHandleIsSignaled()
    {
        using var manualResetEvent = new ManualResetEvent(false);

        var task = manualResetEvent.AsTask();
        task.IsCompleted.Should().BeFalse();

        manualResetEvent.Set();
        await task;

        task.IsCompleted.Should().BeTrue();
        task.Result.Should().BeTrue();
    }

    [Fact]
    public async Task AsTask_WithTimeout_ShouldComplete_WhenWaitHandleIsSignaled()
    {
        using var manualResetEvent = new ManualResetEvent(false);

        var task = manualResetEvent.AsTask(TimeSpan.FromSeconds(1));
        task.IsCompleted.Should().BeFalse();

        manualResetEvent.Set();
        await task;

        task.IsCompleted.Should().BeTrue();
        task.Result.Should().BeTrue();
    }

    [Fact]
    public async Task AsTask_WithTimeout_ShouldCancel_WhenTimeoutIsReached()
    {
        using var manualResetEvent = new ManualResetEvent(false);
        var task = manualResetEvent.AsTask(TimeSpan.FromMilliseconds(100));

        task.IsCompleted.Should().BeFalse();

        Exception? exception = null;
        try
        {
            await task;
        }
        catch (TaskCanceledException ex)
        {
            exception = ex;
        }
        catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
        {
            exception = ex;
        }

        exception.Should().NotBeNull();

        task.IsCanceled.Should().BeTrue();
    }
}
