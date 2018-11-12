namespace System
{
    using System.Collections.Generic;
    using System.Linq;
    using CommonNet.Extensions;
    using ComponentModel;

    /// <summary>
    /// Commonly used extension methods on byte array.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ByteArrayExtensions
    {
        /// <summary>
		/// Returns all occurrences of <paramref name="pattern"/>.
		/// </summary>
        /// <param name="self">Array where perform the search.</param>
        /// <param name="pattern">Pattern to search for.</param>
        /// <returns>Returns all indexes of <paramref name="pattern"/> matches.</returns>
		public static IEnumerable<int> AllIndexesOf(this byte[] self, byte[] pattern)
        {
            Check.Self(self);
            Check.Argument(pattern, nameof(pattern));
            Check.Verify(pattern.Length > 0, $"Argument {nameof(pattern)} is empty.");

            if (pattern.Length == 0)
            {
                yield return 0;
                yield break;
            }

            for (var i = 0; i < self.Length; i++)
            {
                if (self.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        /// Returns first occurrences of <paramref name="pattern"/>.
        /// </summary>
        /// <param name="self">Array where perform the search.</param>
        /// <param name="pattern">Pattern to search for.</param>
        /// <returns>Returns first index of <paramref name="pattern"/> match.</returns>
        public static int IndexOf(this byte[] self, byte[] pattern)
        {
            Check.Self(self);
            Check.Argument(pattern, nameof(pattern));
            Check.Verify(pattern.Length > 0, $"Argument {nameof(pattern)} is empty.");

            var j = -1;
            var end = self.Length - pattern.Length;
            while ((j = Array.IndexOf(self, pattern[0], j + 1)) <= end && j != -1)
            {
                var i = 1;
                while (self[j + i] == pattern[i])
                {
                    if (++i == pattern.Length)
                        return j;
                }
            }
            return -1;
        }

        /// <summary>
        /// XOR byte array with given <paramref name="key"/>.
        /// </summary>
        /// <param name="self">Byte array where to apply XOR.</param>
        /// <param name="index">Index where to start XOR.</param>
        /// <param name="length">Length of data to XOR.</param>
        /// <param name="key">Key for XOR operation.</param>
        public static void Xor(this byte[] self, int index, int length, byte[] key)
        {
            Check.Self(self);
            Check.Argument(key, nameof(key));
            Check.Verify(key.Length > 0 , $"Argument {nameof(key)} is empty.");
            Check.Verify(index >= 0 && index < self.Length, $"Argument {nameof(index)} is out of array bounds.");
            Check.Verify(index + length <= self.Length, $"Argument {nameof(length)} exceeds array bounds.");

            for (int i = index, j = 0; i < index + length; i++, j++)
            {
                self[i] = (byte)(self[i] ^ key[j % key.Length]);
            }
        }
    }
}
