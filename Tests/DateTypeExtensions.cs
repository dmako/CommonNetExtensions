namespace Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DateTimeExtensions
    {
        [Test]
        public void DateTimeExtensions_IsLeapYear()
        {
            Assert.AreEqual(new DateTime(2000, 1, 1).IsLeapYear(), true);
            Assert.AreEqual(new DateTime(1900, 1, 1).IsLeapYear(), false);
            Assert.AreEqual(new DateTime(2011, 1, 1).IsLeapYear(), false);
            Assert.AreEqual(new DateTime(2012, 1, 1).IsLeapYear(), true);
        }

        [Test]
        public void DateTimeExtensions_IsWeekend()
        {
            Assert.AreEqual(new DateTime(2000, 1, 1).IsWeekend(), true);
            Assert.AreEqual(new DateTime(1900, 1, 1).IsWeekend(), false);
            Assert.AreEqual(new DateTime(2011, 1, 1).IsWeekend(), true);
            Assert.AreEqual(new DateTime(2017, 4, 15).IsWeekend(), true);
        }

        [Test]
        public void DateTimeExtensions_ToUnixTimestamp()
        {
            Assert.AreEqual(new DateTime(1970, 1, 1).ToUnixTimestamp(), 0);
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 1).ToUnixTimestamp(), 1);
            Assert.AreEqual(new DateTime(1969, 12, 31, 23, 59, 59).ToUnixTimestamp(), -1);
            Assert.AreEqual(new DateTime(1970, 1, 1, 1, 0, 0).ToUnixTimestamp(), 3600);
        }
    }
}
