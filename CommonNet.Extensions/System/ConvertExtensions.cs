#if NETSTANDARD2_0

using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// Convert Netstandard 2.0 Polyfills
/// </summary>
public class ConvertPolyfills
{
    /// <summary>
    /// Converts an array of bytes to Hex16 string representation encoded with uppercase characters.
    /// </summary>
    /// <param name="inArray">An array of bytes.</param>
    /// <returns>Hex16 string representation encoded with uppercase characters.</returns>
    public static string ToHexString(byte[] inArray)
    {
        Guard.IsNotNull(inArray);

        return ToHexString(inArray.AsReadOnlySpan());
    }

    /// <summary>
    /// Converts an array of bytes to Hex16 string representation encoded with uppercasec haracters.
    /// </summary>
    /// <param name="inArray">An array of bytes.</param>
    /// <param name="offset">An offset in <paramref name="inArray"/>.</param>
    /// <param name="length">The number of elements of <paramref name="inArray"/> to convert.</param>
    /// <returns>Hex16 string representation encoded with uppercase characters.</returns>
    public static string ToHexString(byte[] inArray, int offset, int length)
    {
        Guard.IsNotNull(inArray);
        Guard.IsGreaterThanOrEqualTo(length, 0);
        Guard.IsBetweenOrEqualTo(offset, 0, inArray.Length - length);

        return ToHexString(new ReadOnlySpan<byte>(inArray, offset, length));
    }

    /// <summary>
    /// Converts an array of bytes to Hex16 string representation encoded with uppercase characters.
    /// </summary>
    /// <param name="bytes">A span of bytes.</param>
    /// <returns>Hex16 string representation encoded with uppercase characters.</returns>
    public static string ToHexString(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length == 0)
        {
            return string.Empty;
        }

        return ToHexStringImpl(bytes);
    }

    private static readonly char[] Base16UpperAlphabet = "0123456789ABCDEF".ToArray();

    private static string ToHexStringImpl(ReadOnlySpan<byte> bytes)
    {

        var result = new char[bytes.Length * 2].AsSpan();

        for (int i = 0, o = 0; i < bytes.Length; i++, o += 2)
        {
            var b = bytes[i];
            result[o] = Base16UpperAlphabet[b >> 4];
            result[o + 1] = Base16UpperAlphabet[b & 0x0F];
        }

        return result.ToString();
    }
}

#endif
