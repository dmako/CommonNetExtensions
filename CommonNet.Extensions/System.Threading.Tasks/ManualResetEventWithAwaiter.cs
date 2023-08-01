using System.Runtime.CompilerServices;

namespace System.Threading.Tasks;

/// <summary>
/// Represents a custom manual reset event with awaiter capability, derived from ManualResetEventSlim.
/// </summary>
public sealed class ManualResetEventWithAwaiter : ManualResetEventSlim
{
    private readonly Action _onCompleted;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManualResetEventWithAwaiter"/> class.
    /// The ManualResetEventWithAwaiter is initially not set.
    /// </summary>
    public ManualResetEventWithAwaiter()
    {
        _onCompleted = Set;
    }

    /// <summary>
    /// Waits for the specified awaiter to complete, and then resets the event.
    /// </summary>
    /// <typeparam name="TAwaiter">The type of the awaiter implementing the ICriticalNotifyCompletion interface.</typeparam>
    /// <param name="awaiter">The awaiter to wait for.</param>
    /// <remarks>
    /// The method first waits for the specified awaiter to complete using the UnsafeOnCompleted method.
    /// After the awaiter is completed, it proceeds to wait for the ManualResetEventWithAwaiter to be set,
    /// and then it resets the event before returning.
    /// </remarks>
    public void Wait<TAwaiter>(TAwaiter awaiter)
        where TAwaiter : ICriticalNotifyCompletion
    {
        awaiter.UnsafeOnCompleted(_onCompleted);
        Wait();
        Reset();
    }
}
