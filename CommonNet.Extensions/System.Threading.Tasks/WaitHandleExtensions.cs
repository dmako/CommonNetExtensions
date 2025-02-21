using CommunityToolkit.Diagnostics;

namespace System.Threading.Tasks;

/// <summary>
/// Provides extension methods to convert WaitHandle instances to <see cref="Task{Boolean}"/> for asynchronous waiting.
/// </summary>
public static class WaitHandleExtensions
{
    /// <summary>
    /// Converts a WaitHandle to a <see cref="Task{Boolean}"/> that completes when the WaitHandle is signaled.
    /// </summary>
    /// <param name="handle">The WaitHandle to convert.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> that completes with a value of true when the WaitHandle is signaled,
    /// or false if the WaitHandle is disposed before it is signaled.
    /// </returns>
    public static Task<bool> AsTask(this WaitHandle handle)
    {
        return AsTask(handle, Timeout.InfiniteTimeSpan);
    }

    /// <summary>
    /// Converts a WaitHandle to a <see cref="Task{Boolean}"/> that completes when the WaitHandle is signaled or when the specified timeout is reached.
    /// </summary>
    /// <param name="handle">The WaitHandle to convert.</param>
    /// <param name="timeout">The maximum time to wait for the WaitHandle to be signaled.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> that completes with a value of true when the WaitHandle is signaled,
    /// false if the WaitHandle is disposed before it is signaled, or canceled if the specified timeout is reached.
    /// </returns>
    public static Task<bool> AsTask(this WaitHandle handle, TimeSpan timeout)
    {
        var tcs = new TaskCompletionSource<bool>();
        var registration = ThreadPool.RegisterWaitForSingleObject(handle, (state, timedOut) =>
        {
            Guard.IsNotNull(state);
            
            var localTcs = (TaskCompletionSource<bool>)state;
            if (timedOut)
            {
                localTcs.TrySetCanceled();
            }
            else
            {
                localTcs.TrySetResult(true);
            }
        }, tcs, timeout, true);

        tcs.Task.ContinueWith((_, state) => ((RegisteredWaitHandle)state!).Unregister(null), registration, TaskScheduler.Default);
        return tcs.Task;
    }

#if NET8_0_OR_GREATER

    /// <summary>
    /// Asynchronously waits for a <see cref="WaitHandle"/> to be signaled, with support for cancellation.
    /// </summary>
    /// <param name="waitHandle">The <see cref="WaitHandle"/> to wait for.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the handle to be signaled.</param>
    /// <returns>A <see cref="Task"/> that completes when the <see cref="WaitHandle"/> is signaled or the <paramref name="cancellationToken"/> is canceled.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="waitHandle"/> is null.</exception>
    /// <remarks>
    /// This method registers a callback with the thread pool to wait for the <see cref="WaitHandle"/> to be signaled.
    /// If the <paramref name="cancellationToken"/> is canceled before the handle is signaled, the returned <see cref="Task"/> will be canceled.
    /// </remarks>
    public static Task WaitAsync(this WaitHandle waitHandle, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(waitHandle);

        CancellationTokenRegistration cancellationRegistration = default;

        var tcs = new TaskCompletionSource();
        var handle = ThreadPool.RegisterWaitForSingleObject(
            waitObject: waitHandle,
            callBack: (o, timeout) =>
            {
                _ = cancellationRegistration.Unregister();
                _ = tcs.TrySetResult();
            },
            state: null,
            timeout: Timeout.InfiniteTimeSpan,
            executeOnlyOnce: true);

        if (cancellationToken.CanBeCanceled)
        {
            cancellationRegistration = cancellationToken.Register(() =>
            {
                _ = handle.Unregister(waitHandle);
                _ = tcs.TrySetCanceled(cancellationToken);
            });
        }

        return tcs.Task;
    }

    /// <summary>
    /// Asynchronously waits for a <see cref="ManualResetEventSlim"/> to be set, with support for cancellation.
    /// </summary>
    /// <param name="manualResetEvent">The <see cref="ManualResetEventSlim"/> to wait for.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the event to be set.</param>
    /// <returns>A <see cref="Task"/> that completes when the <see cref="ManualResetEventSlim"/> is set or the <paramref name="cancellationToken"/> is canceled.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="manualResetEvent"/> is null.</exception>
    /// <remarks>
    /// This method calls <see cref="WaitAsync(WaitHandle, CancellationToken)"/> with the <see cref="WaitHandle"/> of the <paramref name="manualResetEvent"/>.
    /// </remarks>
    public static Task WaitAsync(this ManualResetEventSlim manualResetEvent, CancellationToken cancellationToken = default)
        => WaitAsync(manualResetEvent.WaitHandle, cancellationToken);

#endif
}
