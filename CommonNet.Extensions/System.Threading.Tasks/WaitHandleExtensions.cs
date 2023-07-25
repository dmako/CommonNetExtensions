using CommunityToolkit.Diagnostics;

namespace System.Threading.Tasks;

/// <summary>
/// Provides extension methods to convert WaitHandle instances to Task&lt;bool&gt; for asynchronous waiting.
/// </summary>
public static class WaitHandleExtensions
{
    /// <summary>
    /// Converts a WaitHandle to a Task&lt;bool&gt; that completes when the WaitHandle is signaled.
    /// </summary>
    /// <param name="handle">The WaitHandle to convert.</param>
    /// <returns>
    /// A Task&lt;bool&gt; that completes with a value of true when the WaitHandle is signaled,
    /// or false if the WaitHandle is disposed before it is signaled.
    /// </returns>
    public static Task<bool> AsTask(this WaitHandle handle)
    {
        return AsTask(handle, Timeout.InfiniteTimeSpan);
    }

    /// <summary>
    /// Converts a WaitHandle to a Task&lt;bool&gt; that completes when the WaitHandle is signaled or when the specified timeout is reached.
    /// </summary>
    /// <param name="handle">The WaitHandle to convert.</param>
    /// <param name="timeout">The maximum time to wait for the WaitHandle to be signaled.</param>
    /// <returns>
    /// A Task&lt;bool&gt; that completes with a value of true when the WaitHandle is signaled,
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
}
