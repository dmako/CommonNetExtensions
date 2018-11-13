namespace System.Runtime.InteropServices
{
    using System;
    using System.ComponentModel;
    using CommonNet.Extensions;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteropExtensions
    {
        /// <summary>
        /// Marshals data from a managed object to an byte array.
        /// </summary>
        /// <typeparam name="T">Managed object that holds the data to be marshaled. This object must be a structure.</typeparam>
        /// <param name="self">Object to be marshaled to data.</param>
        /// <exception cref="System.ArgumentNullException">In case <paramref name="self"/> is reference type and is null.</exception>
        /// <returns>Byte array with marshaled object data.</returns>
        public static unsafe byte[] StructureToBuffer<T>(this T self)
            where T : unmanaged
        {
            var buffer = new byte[sizeof(T)];
            fixed (byte* bufferPtr = buffer)
            {
                Buffer.MemoryCopy(&self, bufferPtr, sizeof(T), sizeof(T));
            }
            return buffer;
        }

        /// <summary>
        /// Marshals data from byte array to a managed object.
        /// </summary>
        /// <typeparam name="T">Type of structure.</typeparam>
        /// <param name="self"></param>
        /// <exception cref="System.ArgumentNullException">In case <paramref name="self"/> is null.</exception>
        /// <exception cref="System.ArgumentException">In case <paramref name="self"/> length is not equal or greater than size of <typeparamref name="T"/>.</exception>
        /// <returns>The object constructed from data.</returns>
        public static unsafe T BufferToStructure<T>(this byte[] self)
            where T : unmanaged
        {
            Check.Self(self);
            if (self.Length < sizeof(T))
                throw new ArgumentException($"Length of array not equal or greater than size of {typeof(T)}.", nameof(self));

            var result = new T();
            fixed (byte* bufferPtr = self)
            {
                Buffer.MemoryCopy(bufferPtr, &result, sizeof(T), sizeof(T));
            }
            return result;
        }
    }
}
