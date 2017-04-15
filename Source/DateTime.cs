namespace System
{
    using ComponentModel;

    /// <summary>
    /// Commonly used extension methods on <see cref="DateTime"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CommonNetDateTimeExtensions
    {
        /// <summary>
        /// Tests whether the date year is leap.
        /// </summary>
        /// <param name="self">Date to test.</param>
        /// <returns>Returns true in case the year is leap.</returns>
        public static bool IsLeapYear(this DateTime self)
        {
            return (DateTime.DaysInMonth(self.Year, 2) == 29);
        }

        /// <summary>
        /// Tests whether the date is weekend day.
        /// </summary>
        /// <param name="self">Date to test.</param>
        /// <returns>Returns true in case the date is weekend day.</returns>
        public static bool IsWeekend(this DateTime self)
        {
            return self.DayOfWeek == DayOfWeek.Saturday || self.DayOfWeek == DayOfWeek.Sunday;
        }

        private static readonly DateTime unixEpochBeginning = new DateTime(1970, 1, 1, 0, 0, 0);
        /// <summary>
        /// Converts a <see cref="DateTime"/> value to Unix timestamp.
        /// </summary>
        /// <param name="self">Value to convert.</param>
        /// <returns>The 64-bit Unix timestamp. Values before Unix epoch are represented as negative number.</returns>
        public static long ToUnixTimestamp(this DateTime self)
        {
            var ts = self - unixEpochBeginning;
            return (long)ts.TotalSeconds;
        }
    }
}