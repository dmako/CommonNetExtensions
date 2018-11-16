namespace System
{
    using System.Collections.Generic;
    using CommonNet.Extensions;
    using ComponentModel;

    /// <summary>
    /// Commonly used extension methods on spans.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SpanExtensions
    {
        /// <summary>
		/// Returns all occurrences of <paramref name="pattern"/>.
		/// </summary>
        /// <param name="self">Span where perform the search.</param>
        /// <param name="pattern">Pattern to search for.</param>
        /// <returns>Returns all indexes of <paramref name="pattern"/> matches.</returns>
		public static IEnumerable<int> AllIndexesOf<T>(this ReadOnlySpan<T> self, ReadOnlySpan<T> pattern)
            where T : IEquatable<T>
        {
            Check.VerifyArgument(nameof(pattern), pattern.Length > 0, $"Argument {nameof(pattern)} is empty.");

            var results = new List<int>();

            for (var i = 0; i < self.Length; i++)
            {
                if (self.Slice(i).StartsWith(pattern))
                {
                    results.Add(i);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns first occurrences of <paramref name="pattern"/>.
        /// </summary>
        /// <param name="self">Span where perform the search.</param>
        /// <param name="pattern">Pattern to search for.</param>
        /// <returns>Returns first index of <paramref name="pattern"/> match.</returns>
        public static int IndexOf<T>(this ReadOnlySpan<T> self, ReadOnlySpan<T> pattern)
            where T : IEquatable<T>
        {
            Check.VerifyArgument(nameof(pattern), pattern.Length > 0, $"Argument {nameof(pattern)} is empty.");

            for (var i = 0; i < self.Length; i++)
            {
                if (self.Slice(i).StartsWith(pattern))
                {
                    return i;
                }
            }

            return -1;
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
            Check.VerifyArgument(nameof(key), key.Length > 0 , $"Argument {nameof(key)} is empty.");
            Check.VerifyArgument(nameof(index), index >= 0 && index < self.Length, $"Argument {nameof(index)} is out of array bounds.");
            Check.VerifyArgument(nameof(length), index + length <= self.Length, $"Argument {nameof(length)} exceeds array bounds.");

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
            Check.Self(self);
            Xor(new Span<byte>(self), index, length, key);
        }
    }
}
