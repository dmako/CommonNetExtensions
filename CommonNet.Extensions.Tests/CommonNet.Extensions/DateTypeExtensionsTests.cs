namespace CommonNet.Extensions.Tests;

public class DateTimeExtensions
{
    [Test]
    public async Task DateTimeExtensions_IsLeapYear()
    {
        await Assert.That(new DateTime(2000, 1, 1).IsLeapYear())
            .IsTrue();
        await Assert.That(new DateTime(1900, 1, 1).IsLeapYear())
            .IsFalse();
        await Assert.That(new DateTime(2011, 1, 1).IsLeapYear())
            .IsFalse();
        await Assert.That(new DateTime(2012, 1, 1).IsLeapYear())
            .IsTrue();
    }

    [Test]
    public async Task DateTimeExtensions_IsWeekend()
    {
        await Assert.That(new DateTime(2000, 1, 1).IsWeekend())
            .IsTrue();
        await Assert.That(new DateTime(1900, 1, 1).IsWeekend())
            .IsFalse();
        await Assert.That(new DateTime(2011, 1, 1).IsWeekend())
            .IsTrue();
        await Assert.That(new DateTime(2017, 4, 15).IsWeekend())
            .IsTrue();
    }

    [Test]
    public async Task DateTimeExtensions_ToUnixTimestamp()
    {
        await Assert.That(new DateTime(1970, 1, 1).ToUnixTimestamp())
            .IsEqualTo(0);
        await Assert.That(new DateTime(1970, 1, 1, 0, 0, 1).ToUnixTimestamp())
            .IsEqualTo(1);
        await Assert.That(new DateTime(1969, 12, 31, 23, 59, 59).ToUnixTimestamp())
            .IsEqualTo(-1);
        await Assert.That(new DateTime(1970, 1, 1, 1, 0, 0).ToUnixTimestamp())
            .IsEqualTo(3600);
    }
}
