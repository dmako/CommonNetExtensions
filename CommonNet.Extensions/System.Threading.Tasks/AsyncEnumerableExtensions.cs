#if !NET7_0_OR_GREATER

using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;

namespace System.Threading.Tasks;

/// <summary>
/// Extension methods for asynchronous enumerables.
/// </summary>
public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// Netstandard2.0 abd .Net 6 polyfill extension method that converts an <see cref="IAsyncEnumerable{T}"/> instance into an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The enumerated type.</typeparam>
    /// <param name="asyncEnum">The source enumerable.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> instance that enumerates the source <see cref="IAsyncEnumerable{T}"/> in a blocking manner.</returns>
    /// <remarks>
    /// This method is implemented by using deferred execution. The underlying <see cref="IAsyncEnumerable{T}"/> will not be enumerated
    /// unless the returned <see cref="IEnumerable{T}"/> is enumerated by calling its <see cref="IEnumerable{T}.GetEnumerator"/> method.
    /// Async enumeration does not happen in the background; each MoveNext call will invoke the underlying <see cref="IAsyncEnumerator{T}.MoveNextAsync"/> exactly once.
    /// </remarks>
    public static IEnumerable<T> ToBlockingEnumerable<T>(this IAsyncEnumerable<T> asyncEnum, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(asyncEnum);

        var enumerator = asyncEnum.GetAsyncEnumerator(cancellationToken);
        ManualResetEventWithAwaiter? manResEvt = null;

        try
        {
            while (true)
            {
#pragma warning disable CA2012 // Intended here
                var moveNextTask = enumerator.MoveNextAsync();
#pragma warning restore CA2012

                if (!moveNextTask.IsCompleted)
                {
                    manResEvt ??= new ManualResetEventWithAwaiter();
                    manResEvt.Wait(moveNextTask.GetAwaiter());
                }

                if (!moveNextTask.IsCompletedSuccessfully || !moveNextTask.Result)
                {
                    yield break;
                }

                yield return enumerator.Current;
            }
        }
        finally
        {
            var disposeTask = enumerator.DisposeAsync();

            if (!disposeTask.IsCompleted)
            {
                manResEvt ??= new ManualResetEventWithAwaiter();
                manResEvt.Wait(disposeTask.GetAwaiter());
            }

            disposeTask.GetAwaiter().GetResult();
        }
    }
}

#endif
