namespace CommonNet.Extensions.Tests
{
    using System;
    using Xunit;

    public class DateTimeExtensions
    {
        [Fact]
        public void DateTimeExtensions_IsLeapYear()
        {
            Assert.True(new DateTime(2000, 1, 1).IsLeapYear());
            Assert.False(new DateTime(1900, 1, 1).IsLeapYear());
            Assert.False(new DateTime(2011, 1, 1).IsLeapYear());
            Assert.True(new DateTime(2012, 1, 1).IsLeapYear());
        }

        [Fact]
        public void DateTimeExtensions_IsWeekend()
        {
            Assert.True(new DateTime(2000, 1, 1).IsWeekend());
            Assert.False(new DateTime(1900, 1, 1).IsWeekend());
            Assert.True(new DateTime(2011, 1, 1).IsWeekend());
            Assert.True(new DateTime(2017, 4, 15).IsWeekend());
        }

        [Fact]
        public void DateTimeExtensions_ToUnixTimestamp()
        {
            Assert.Equal(0, new DateTime(1970, 1, 1).ToUnixTimestamp());
            Assert.Equal(1, new DateTime(1970, 1, 1, 0, 0, 1).ToUnixTimestamp());
            Assert.Equal(-1, new DateTime(1969, 12, 31, 23, 59, 59).ToUnixTimestamp());
            Assert.Equal(3600, new DateTime(1970, 1, 1, 1, 0, 0).ToUnixTimestamp());
        }
    }
}
