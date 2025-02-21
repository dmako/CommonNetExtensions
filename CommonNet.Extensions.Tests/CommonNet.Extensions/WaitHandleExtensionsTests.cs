#pragma warning disable xUnit1031

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


#if NET8_0_OR_GREATER

    [Fact]
    public async Task WaitAsync_WaitHandle_Signaled_CompletesTask()
    {
        using var manualEvent = new ManualResetEvent(false);
        WaitHandle waitHandle = manualEvent;
        var cancellationToken = CancellationToken.None;

        var task = waitHandle.WaitAsync(cancellationToken);
        manualEvent.Set();

        await task.Awaiting(t => t).Should().NotThrowAsync();
    }

    [Fact]
    public async Task WaitAsync_WaitHandle_Canceled_ThrowsTaskCanceledException()
    {
        using var manualEvent = new ManualResetEvent(false);
        WaitHandle waitHandle = manualEvent;
        var cts = new CancellationTokenSource();

        var task = waitHandle.WaitAsync(cts.Token);
        cts.Cancel();

        await task.Awaiting(t => t).Should().ThrowAsync<TaskCanceledException>();
    }

    [Fact]
    public async Task WaitAsync_ManualResetEventSlim_Signaled_CompletesTask()
    {

        using var manualResetEvent = new ManualResetEventSlim(false);
        var cancellationToken = CancellationToken.None;

        var task = manualResetEvent.WaitAsync(cancellationToken);
        manualResetEvent.Set();

        await task.Awaiting(t => t).Should().NotThrowAsync();
    }

    [Fact]
    public async Task WaitAsync_ManualResetEventSlim_Canceled_ThrowsTaskCanceledException()
    {
        // Arrange
        using var manualResetEvent = new ManualResetEventSlim(false);
        var cts = new CancellationTokenSource();

        var task = manualResetEvent.WaitAsync(cts.Token);
        cts.Cancel();

        await task.Awaiting(t => t).Should().ThrowAsync<TaskCanceledException>();
    }

    [Fact]
    public void WaitAsync_WaitHandle_Null_ThrowsArgumentNullException()
    {
        WaitHandle waitHandle = null!;
        var cancellationToken = CancellationToken.None;

        var func = () => waitHandle.WaitAsync(cancellationToken);
        func.Should().ThrowAsync<ArgumentException>();
    }

#endif

}

#pragma warning restore xUnit1031
