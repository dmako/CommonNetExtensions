namespace System
{
    using System.Linq;
    using Collections.Generic;
    using CommonNet.Extensions;
    using ComponentModel;
    using Globalization;
    using Reflection;

    /// <summary>
    /// Commonly used extension methods on <see cref="string"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CommonNetStringExtensions
    {
        /// <summary>
        /// Indicates whether the specified string is not null and differs from <see cref="string.Empty"/> string.
        /// </summary>
        /// <param name="self">The string to test.</param>
        /// <returns>false if the string is null or an empty string, true otherwise.</returns>
        public static bool IsNotNullOrEmpty(this string self) => !string.IsNullOrEmpty(self);

        /// <summary>
        /// Indicates whether a specified string is not null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="self">The string to test.</param>
        /// <returns>false if the string is null or empty string, or if consists only of white-space characters, true otherwise.</returns>
        public static bool IsNotNullOrWhiteSpace(this string self) => !string.IsNullOrWhiteSpace(self);

        // helper data to substitute TypeDescriptor.GetConverter() functionality that is not available on PCL
        private static readonly Dictionary<Type, Func<string, object>> parsers =
            new Dictionary<Type, Func<string, object>>
        {
            { typeof(bool), (s) => bool.Parse(s) },
            { typeof(sbyte), (s) => sbyte.Parse(s) },
            { typeof(byte), (s) => byte.Parse(s) },
            { typeof(char), (s) => s[0] },
            { typeof(short), (s) => short.Parse(s) },
            { typeof(ushort), (s) => ushort.Parse(s) },
            { typeof(int), (s) => int.Parse(s) },
            { typeof(uint), (s) => uint.Parse(s) },
            { typeof(long), (s) => long.Parse(s) },
            { typeof(ulong), (s) => ulong.Parse(s) },
            { typeof(double), (s) => double.Parse(s, CultureInfo.InvariantCulture) },
            { typeof(DateTime), (s) => DateTime.Parse(s, CultureInfo.InvariantCulture) },
            { typeof(TimeSpan), (s) => TimeSpan.Parse(s, CultureInfo.InvariantCulture) },
            { typeof(Guid), (s) => Guid.Parse(s) },
            { typeof(Version), (s) => Version.Parse(s) },
        };

        /// <summary>
        /// Parse string into desired type.
        /// </summary>
        /// <typeparam name="T">Deduced parameter by return type or explicitly typed one.</typeparam>
        /// <param name="self">The string to parse.</param>
        /// <returns></returns>
        public static T Parse<T>(
            this string self
        )
        {
            Check.Self(self);

            var result = default(T);
            if (self.IsNotNullOrEmpty())
            {
                Func<string, object> convertFnc;
                if (!parsers.TryGetValue(typeof(T), out convertFnc))
                {
                    throw new NotSupportedException();
                }
                result = (T)convertFnc(self);
            }
            return result;
        }

        /// <summary>
        /// Try parse string into desired type.
        /// </summary>
        /// <param name="self">The string to parse.</param>
        /// <param name="value">Value where to store result of parse.</param>
        /// <returns></returns>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TryParse<T>(
            this string self,
            out T value
        )
        {
            try
            {
                value = self.Parse<T>();
            }
            catch (Exception e)
            {
                if (e is OverflowException || e is NotSupportedException)
                {
                    value = default(T);
                    return false;
                }
                throw;
            }
            return true;
        }

        /// <summary>
        /// Parse a string to a enum value if string matches enum definition name otherwise default value is returned.
        /// For extensive usage consider https://github.com/TylerBrinkley/Enums.NET
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="self"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static TEnum ParseToEnum<TEnum>(this string self, bool ignoreCase = true)
            where TEnum : struct
        {
            Check.Verify(typeof(TEnum).GetTypeInfo().IsEnum, $"{typeof(TEnum).Name} must be Enum.");
            TEnum eval;
            if (Enum.TryParse(self, ignoreCase, out eval))
                return eval;
            return default(TEnum);
        }

        /// <summary>
        /// Generates a sequence that contains a repeated string value optionally concatenated with separator.
        /// </summary>
        /// <param name="self">The string to repeat.</param>
        /// <param name="count">Number of repetitions.</param>
        /// <param name="separator">Optional separator of repeated string value.</param>
        /// <returns></returns>
        public static string Repeat(this string self, int count, string separator = "")
        {
            Check.Self(self);

            if (count < 0)
                throw new ArgumentException("Must be a positive number.", nameof(count));

            if (self.Length > 0 && count > 0)
                return string.Join(separator, Enumerable.Repeat(self, count).ToArray());

            return string.Empty;
        }

        /// <summary>
        /// Replaces all tabulator occurrences in string with given number of spaces.
        /// </summary>
        /// <param name="self">The string on which to perform replacement.</param>
        /// <param name="tabSize">Number of spaces to use for replacing tabulator.</param>
        /// <returns></returns>
        public static string TabsToSpaces(this string self, int tabSize = 4)
        {
            Check.Self(self);

            if (tabSize < 0)
                throw new ArgumentException($"Parameter {nameof(tabSize)} cannot be negative");

            return self.Replace("\t", " ".Repeat(tabSize));
        }

        /// <summary>
        /// Gets the string before the given string parameter.
        /// </summary>
        /// <param name="self">The string on which to perform.</param>
        /// <param name="right">The string to seek.</param>
        /// <returns>
        /// Returns string before occurrence of parameter <paramref name="right"/>
        /// or empty string in case the parameter <paramref name="right"/> is not in <paramref name="self"/> string.
        /// </returns>
        public static string GetBeforeOrEmpty(this string self, string right)
        {
            Check.Self(self);
            Check.Verify(right.IsNotNullOrEmpty(), $"Argument {nameof(right)} cannot be null or empty.");

            var rightPos = self.IndexOf(right, StringComparison.Ordinal);
            return rightPos == -1 ? string.Empty : self.Substring(0, rightPos);
        }

        /// <summary>
        /// Gets the string after the given string parameter.
        /// </summary>
        /// <param name="self">The string on which to perform.</param>
        /// <param name="left">The string to seek.</param>
        /// <returns>
        /// Returns string after occurrence of parameter <paramref name="left"/>
        /// or empty string in case the parameter <paramref name="left"/> is not in <paramref name="self"/> string.
        /// </returns>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public static string GetAfterOrEmpty(this string self, string left)
        {
            Check.Self(self);
            Check.Verify(left.IsNotNullOrEmpty(), $"Argument {nameof(left)} cannot be null or empty.");

            var leftPos = self.LastIndexOf(left, StringComparison.Ordinal);
            return leftPos == -1 || leftPos + left.Length >= self.Length ?
                string.Empty :
                self.Substring(leftPos + left.Length);
        }

        /// <summary>
        /// Gets the string between the given string parameters.
        /// </summary>
        /// <param name="self">he string on which to perform.</param>
        /// <param name="left">The string to seek at the beginning.</param>
        /// <param name="right">The string to seek from the end.</param>
        /// <returns>
        /// Returns string after occurrence of parameter <paramref name="left"/> and before parameter <paramref name="right"/>
        /// or empty string in case the parameter <paramref name="left"/> of parameter <paramref name="right"/> is not in <paramref name="self"/> string.
        /// </returns>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public static string GetBetweenOrEmpty(this string self, string left, string right)
        {
            Check.Self(self);
            Check.Verify(left.IsNotNullOrEmpty(), $"Argument {nameof(left)} cannot be null or empty.");
            Check.Verify(right.IsNotNullOrEmpty(), $"Argument {nameof(right)} cannot be null or empty.");

            var leftPos = self.IndexOf(left, StringComparison.Ordinal);
            var rightPos = self.LastIndexOf(right, StringComparison.Ordinal);

            if (leftPos == -1 || leftPos == -1)
                return string.Empty;

            var startIndex = leftPos + left.Length;
            return startIndex >= rightPos ? string.Empty : self.Substring(startIndex, rightPos - startIndex);
        }

        /// <summary>
        /// Returns all zero-based indexes of the occurrences of the specified string
        /// in the string object.
        /// </summary>
        /// <param name="self">The object where to perform search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="ignoreCase">Indicates whether the comparison should be performed with case ignoring.</param>
        /// <returns>Enumeration of zero-based indexes.</returns>
        public static IEnumerable<int> AllIndexesOf(this string self, string value, bool ignoreCase = false)
        {
            Check.Self(self);
            Check.Self(value);
            Check.Verify(value.Length > 0, $"Argument {nameof(value)} is empty.");

            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            for (var index = 0; ; ++index)
            {
                index = self.IndexOf(value, index, comparison);
                if (index == -1)
                    break;
                yield return index;
            }
        }
    }
}
