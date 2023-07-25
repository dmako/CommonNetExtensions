using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;
public class DateTimeExtensions
{
    [Fact]
    public void DateTimeExtensions_IsLeapYear()
    {
        new DateTime(2000, 1, 1).IsLeapYear().Should().BeTrue();
        new DateTime(1900, 1, 1).IsLeapYear().Should().BeFalse();
        new DateTime(2011, 1, 1).IsLeapYear().Should().BeFalse();
        new DateTime(2012, 1, 1).IsLeapYear().Should().BeTrue();
    }

    [Fact]
    public void DateTimeExtensions_IsWeekend()
    {
        new DateTime(2000, 1, 1).IsWeekend().Should().BeTrue();
        new DateTime(1900, 1, 1).IsWeekend().Should().BeFalse();
        new DateTime(2011, 1, 1).IsWeekend().Should().BeTrue();
        new DateTime(2017, 4, 15).IsWeekend().Should().BeTrue();
    }

    [Fact]
    public void DateTimeExtensions_ToUnixTimestamp()
    {
        new DateTime(1970, 1, 1).ToUnixTimestamp().Should().Be(0);
        new DateTime(1970, 1, 1, 0, 0, 1).ToUnixTimestamp().Should().Be(1);
        new DateTime(1969, 12, 31, 23, 59, 59).ToUnixTimestamp().Should().Be(-1);
        new DateTime(1970, 1, 1, 1, 0, 0).ToUnixTimestamp().Should().Be(3600);
    }
}
