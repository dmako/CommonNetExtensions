namespace System
{
    /// <summary>
    /// Extension methods for value types.
    /// </summary>
    public static class ValueTypesExtensions
    {
        /// <summary>
        /// Tests value whether is empty.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="self">The value to perform test on.</param>
        /// <returns>True in case the value is equal to default value.</returns>
        public static bool IsEmpty<T>(this T self)
            where T : struct
        {
            return self.Equals(default(T));
        }

        /// <summary>
        /// Tests value whether is not empty.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="self">The value to perform test on.</param>
        /// <returns>True in case the value is not equal to default value.</returns>
        public static bool IsNotEmpty<T>(this T self)
            where T : struct
        {
            return (self.IsEmpty() == false);
        }

        /// <summary>
        /// Casts value to <see cref="System.Nullable{T}"/> object.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="self">The value for casting.</param>
        /// <returns>Nullable object or null in case the value is equal to type default value.</returns>
        public static T? ToNullable<T>(this T self)
            where T : struct
        {
            return (self.IsEmpty() ? null : (T?)self);
        }
    }
}
