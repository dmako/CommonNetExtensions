using System.ComponentModel;
using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// Provides extension methods for <see cref="Exception"/> objects.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ExceptionExtensions
{
    /// <summary>
    /// Adds a key and value(s) to the Data property of an exception.
    /// </summary>
    /// <typeparam name="T">The type of the exception. Must be or derive from <see cref="Exception"/>.</typeparam>
    /// <param name="exception">The exception to which the data will be added.</param>
    /// <param name="key">The key under which the data will be stored.</param>
    /// <param name="values">The value(s) to store. Can be a single value or multiple values. If multiple values are provided, they are stored as an array.</param>
    /// <returns>The exception with the added data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/> or <paramref name="key"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="key"/> is an empty string.</exception>
    public static T AddData<T>(this T exception, string key, params object[] values) where T : Exception
    {
        Guard.IsNotNull(exception);
        Guard.IsNotEmpty(key);

        if (values is { Length: 1})
        {
            exception.Data.Add(key, values[0]);
        }
        else if(values is { Length: > 1 })
        {
            exception.Data.Add(key, values);
        }
        else
        {
            exception.Data.Add(key, null);
        }

        return exception;
    }
}
