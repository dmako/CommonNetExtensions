using System.ComponentModel;
using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// Commonly used extension methods on spans.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ArrayAndSpanExtensions
{
    /// <summary>
    /// Returns ReadOnlySpan containing array.
    /// </summary>
    /// <param name="self">Array for embedding.</param>
    /// <returns>Returns ReadOnlySpan.</returns>
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] self)
    {
        Guard.IsNotNull(self);

        return new ReadOnlySpan<T>(self);
    }

    /// <summary>
    /// Returns all occurrences of <paramref name="pattern"/>.
    /// </summary>
    /// <param name="self">Span where perform the search.</param>
    /// <param name="pattern">Pattern to search for.</param>
    /// <returns>Returns all indexes of <paramref name="pattern"/> matches.</returns>
    public static IEnumerable<int> AllIndexesOf<T>(this ReadOnlySpan<T> self, ReadOnlySpan<T> pattern)
    where T : IEquatable<T>
    {
        Guard.IsNotEmpty(pattern);

        var results = new List<int>();

        for (var i = 0; i < self.Length; i++)
        {
            if (self[i..].StartsWith(pattern))
            {
                results.Add(i);
            }
        }

        return results;
    }

    /// <summary>
    /// XOR byte span with given <paramref name="key"/>.
    /// </summary>
    /// <param name="self">Byte span where to apply XOR.</param>
    /// <param name="index">Index where to start XOR.</param>
    /// <param name="length">Length of data to XOR.</param>
    /// <param name="key">Key for XOR operation.</param>
    public static void Xor(this Span<byte> self, int index, int length, ReadOnlySpan<byte> key)
    {
        Guard.IsNotEmpty(key);
        Guard.IsBetweenOrEqualTo(index, 0, self.Length);
        Guard.IsLessThanOrEqualTo(index + length, self.Length);

        for (int i = index, j = 0; i < index + length; i++, j++)
        {
            self[i] = (byte)(self[i] ^ key[j % key.Length]);
        }
    }

    /// <summary>
    /// XOR byte array with given <paramref name="key"/>.
    /// </summary>
    /// <remarks>Trampoline function to not explicitly construct span on self object.</remarks>
    /// <param name="self">Byte array where to apply XOR.</param>
    /// <param name="index">Index where to start XOR.</param>
    /// <param name="length">Length of data to XOR.</param>
    /// <param name="key">Key for XOR operation.</param>

    public static void Xor(this byte[] self, int index, int length, ReadOnlySpan<byte> key)
    {
        Guard.IsNotNull(self);
        Xor(new Span<byte>(self), index, length, key);
    }
}
