﻿using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// Commonly used extension methods on <see cref="string"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class StringExtensions
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

    // helper data to substitute TypeDescriptor.GetConverter() functionality that is not available on BCL
    private static readonly Dictionary<Type, Func<string, object>> parsers =
        new()
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
        Guard.IsNotNull(self);

        var result = default(T);
        if (self.IsNotNullOrEmpty())
        {
            if (!parsers.TryGetValue(typeof(T), out var convertFnc))
            {
                throw new NotSupportedException();
            }
            result = (T)convertFnc(self);
        }
        return result!;
    }

    /// <summary>
    /// Try parse string into desired type.
    /// </summary>
    /// <param name="self">The string to parse.</param>
    /// <param name="value">Value where to store result of parse.</param>
    /// <returns></returns>
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
            if (e is OverflowException || e is NotSupportedException || e is FormatException)
            {
                value = default!;
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
        if (Enum.TryParse<TEnum>(self, ignoreCase, out var eval))
            return eval;
        return default;
    }

    /// <summary>
    /// Generates a sequence that contains a repeated string value optionally concatenated with separator.
    /// </summary>
    /// <param name="self">The string to repeat.</param>
    /// <param name="numberOfRepetitions">Number of repetitions.</param>
    /// <param name="separator">Optional separator of repeated string value.</param>
    /// <returns></returns>
    public static string Repeat(this string self, int numberOfRepetitions, string separator = "")
    {
        Guard.IsNotNull(self);
        Guard.IsGreaterThanOrEqualTo(numberOfRepetitions, 0);

        if (self is { Length: > 0 } && numberOfRepetitions > 0)
        {
            return string.Join(separator, Enumerable.Repeat(self, numberOfRepetitions).ToArray());
        }
        return string.Empty;
    }

    /// <summary>
    /// Repeats the specified character a given number of times.
    /// </summary>
    /// <param name="self">The character to repeat.</param>
    /// <param name="numberOfRepetitions">The number of times to repeat the character. Must be greater than or equal to 0.</param>
    /// <returns>A new string consisting of the specified character repeated the specified number of times. Returns an empty string if <paramref name="numberOfRepetitions"/> is 0.</returns>
    public static string Repeat(this char self, int numberOfRepetitions)
    {
        Guard.IsGreaterThanOrEqualTo(numberOfRepetitions, 0);

        return numberOfRepetitions == 0 ?
            string.Empty :
            new string(Enumerable.Repeat(self, numberOfRepetitions).ToArray());
    }

    /// <summary>
    /// Replaces all tabulator occurrences in string with given number of spaces.
    /// </summary>
    /// <param name="self">The string on which to perform replacement.</param>
    /// <param name="tabSize">Number of spaces to use for replacing tabulator.</param>
    /// <returns></returns>
    public static string TabsToSpaces(this string self, int tabSize = 4)
    {
        Guard.IsNotNull(self);
        Guard.IsGreaterThan(tabSize, 0);

        return self.Replace("\t", " ".Repeat(tabSize));
    }

    /// <summary>
    /// Pads the right side of the string with a specified character up to a total specified length.
    /// </summary>
    /// <param name="self">The original string to pad.</param>
    /// <param name="totalLength">The total length of the string after padding.</param>
    /// <param name="padCharacter">The character to pad on the right side of the string. Defaults to a space character if not specified.</param>
    /// <returns>A new string that is right-padded with the <paramref name="padCharacter"/> up to the <paramref name="totalLength"/>.</returns>
    public static string PadRight(this string self, int totalLength, char padCharacter = ' ')
    {
        Guard.IsNotNull(self);
        Guard.IsGreaterThan(totalLength, 0);

        return self + padCharacter.Repeat(Math.Max(totalLength - self.Length, 0));
    }

    /// <summary>
    /// Pads the left side of the string with a specified character up to a total specified length.
    /// </summary>
    /// <param name="self">The original string to pad.</param>
    /// <param name="totalLength">The total length of the string after padding.</param>
    /// <param name="padCharacter">The character to pad on the left side of the string. Defaults to a space character if not specified.</param>
    /// <returns>A new string that is left-padded with the <paramref name="padCharacter"/> up to the <paramref name="totalLength"/>.</returns>
    public static string PadLeft(this string self, int totalLength, char padCharacter = ' ')
    {
        Guard.IsNotNull(self);
        Guard.IsGreaterThan(totalLength, 0);

        return padCharacter.Repeat(Math.Max(totalLength - self.Length, 0)) + self;
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
        Guard.IsNotNull(self);
        Guard.IsNotNullOrEmpty(right);

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
    public static string GetAfterOrEmpty(this string self, string left)
    {
        Guard.IsNotNull(self);
        Guard.IsNotNullOrEmpty(left);

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
    public static string GetBetweenOrEmpty(this string self, string left, string right)
    {
        Guard.IsNotNull(self);
        Guard.IsNotNullOrEmpty(left);
        Guard.IsNotNullOrEmpty(right);

        var leftPos = self.IndexOf(left, StringComparison.Ordinal);
        var rightPos = self.LastIndexOf(right, StringComparison.Ordinal);

        if (leftPos == -1 || rightPos == -1)
        {
            return string.Empty;
        }

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
        Guard.IsNotNull(self);
        Guard.IsNotNullOrEmpty(value);

        var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var index = 0;
        while (true)
        {
            index = self.IndexOf(value, index, comparison);
            if (index == -1)
            {
                break;
            }
            yield return index;
            ++index;
        }
    }
}
