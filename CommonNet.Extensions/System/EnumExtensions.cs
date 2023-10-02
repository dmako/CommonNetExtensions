using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// <see cref="System.Enum"/> extensions
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Determines whether the provided enumeration value is one of the specified enum values.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <param name="enumeration">The enumeration value to check.</param>
    /// <param name="enums">An array of enum values to compare against.</param>
    /// <returns>
    ///   <c>true</c> if the <paramref name="enumeration"/> is one of the <paramref name="enums"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enums"/> is null.</exception>
    /// <remarks>
    /// This extension method is used to check if the provided enumeration value is one of the specified enum values.
    /// It compares the <paramref name="enumeration"/> against each value in the <paramref name="enums"/> array and returns true if
    /// a match is found, otherwise, it returns false.
    /// </remarks>
    public static bool IsOneOf<TEnum>(this TEnum enumeration, params TEnum[] enums)
        where TEnum : Enum
    {
        Guard.IsNotNull(enums);

        return enums.Contains(enumeration);
    }
}
