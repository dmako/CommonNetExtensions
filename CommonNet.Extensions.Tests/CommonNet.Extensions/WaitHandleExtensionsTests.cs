using TUnit.Assertions.AssertConditions.Throws;

namespace CommonNet.Extensions.Tests;

public class WaitHandleExtensionsTests
{
    [Test]
    public async Task AsTask_ShouldComplete_WhenWaitHandleIsSignaled()
    {
        using var manualResetEvent = new ManualResetEvent(false);

        var task = manualResetEvent.AsTask();
        await Assert.That(task.IsCompleted)
            .IsFalse();

        manualResetEvent.Set();
        await task;

        await Assert.That(task.IsCompleted)
            .IsTrue();
        await Assert.That(task.Result)
            .IsTrue();
    }

    [Test]
    public async Task AsTask_WithTimeout_ShouldComplete_WhenWaitHandleIsSignaled()
    {
        using var manualResetEvent = new ManualResetEvent(false);

        var task = manualResetEvent.AsTask(TimeSpan.FromSeconds(1));
        await Assert.That(task.IsCompleted)
            .IsFalse();

        manualResetEvent.Set();
        await task;

        await Assert.That(task.IsCompleted)
            .IsTrue();
        await Assert.That(task.Result)
            .IsTrue();
    }

    [Test]
    public async Task AsTask_WithTimeout_ShouldCancel_WhenTimeoutIsReached()
    {
        using var manualResetEvent = new ManualResetEvent(false);
        var task = manualResetEvent.AsTask(TimeSpan.FromMilliseconds(100));

        await Assert.That(task.IsCompleted)
            .IsFalse();

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

        await Assert.That(exception)
            .IsNotNull();

        await Assert.That(task.IsCanceled)
            .IsTrue();
    }


#if NET8_0_OR_GREATER

    [Test]
    public async Task WaitAsync_WaitHandle_Signaled_CompletesTask()
    {
        using var manualEvent = new ManualResetEvent(false);
        WaitHandle waitHandle = manualEvent;
        var cancellationToken = CancellationToken.None;

        var task = waitHandle.WaitAsync(cancellationToken);
        manualEvent.Set();

        await Assert.That(async () => await task)
            .ThrowsNothing();
    }

    [Test]
    public async Task WaitAsync_WaitHandle_Canceled_ThrowsTaskCanceledException()
    {
        using var manualEvent = new ManualResetEvent(false);
        WaitHandle waitHandle = manualEvent;
        var cts = new CancellationTokenSource();

        var task = waitHandle.WaitAsync(cts.Token);
        cts.Cancel();

        await Assert.That(async () => await task)
            .ThrowsExactly<TaskCanceledException>();
    }

    [Test]
    public async Task WaitAsync_ManualResetEventSlim_Signaled_CompletesTask()
    {

        using var manualResetEvent = new ManualResetEventSlim(false);
        var cancellationToken = CancellationToken.None;

        var task = manualResetEvent.WaitAsync(cancellationToken);
        manualResetEvent.Set();

        await Assert.That(async () => await task)
            .ThrowsNothing();
    }

    [Test]
    public async Task WaitAsync_ManualResetEventSlim_Canceled_ThrowsTaskCanceledException()
    {
        // Arrange
        using var manualResetEvent = new ManualResetEventSlim(false);
        var cts = new CancellationTokenSource();

        var task = manualResetEvent.WaitAsync(cts.Token);
        cts.Cancel();

        await Assert.That(async () => await task)
            .ThrowsExactly<TaskCanceledException>();
    }

    [Test]
    public async Task WaitAsync_WaitHandle_Null_ThrowsArgumentNullException()
    {
        WaitHandle waitHandle = null!;
        var cancellationToken = CancellationToken.None;

        await Assert.That(async () => await waitHandle.WaitAsync(cancellationToken))
            .Throws<ArgumentException>();
    }

#endif

}
