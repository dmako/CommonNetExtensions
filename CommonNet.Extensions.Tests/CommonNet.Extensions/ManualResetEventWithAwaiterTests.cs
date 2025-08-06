namespace CommonNet.Extensions.Tests;

public class ManualResetEventWithAwaiterTests
{
    [Test]
    public async Task ManualResetEventWithAwaiter_ShouldWaitForAwaiterAndReset()
    {
        var count = 0;
        var asyncFunc = async () => { await Task.Delay(TimeSpan.FromMilliseconds(1)); count++; };
        var mre = new ManualResetEventWithAwaiter();

        var awaiter = asyncFunc().GetAwaiter();
        mre.Wait(awaiter);

        await Assert.That(awaiter.IsCompleted)
            .IsTrue();
        await Assert.That(mre.IsSet)
            .IsFalse();
        await Assert.That(count)
            .IsEqualTo(1);

        awaiter = asyncFunc().GetAwaiter();
        mre.Wait(awaiter);

        await Assert.That(awaiter.IsCompleted)
            .IsTrue();
        await Assert.That(mre.IsSet)
            .IsFalse();
        await Assert.That(count)
            .IsEqualTo(2);
    }
}
