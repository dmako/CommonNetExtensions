#if NETSTANDARD2_0

using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// Netstandard 2.0 IDisposable polyfills for calling DisposeAsync()
/// </summary>
public static class AsyncDisposablePolyfillsExtensions
{
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
    /// </summary>
    /// <param name="disposable">The IDisposable object to perform the call on.</param>
    /// <returns></returns>
    public static ValueTask DisposeAsync(this IDisposable disposable)
    {
        Guard.IsNotNull(disposable);

        try
        {
            disposable.Dispose();
            return default;
        }
        catch (Exception ex)
        {
            return new ValueTask(Task.FromException(ex));
        }
    }
}

#endif
