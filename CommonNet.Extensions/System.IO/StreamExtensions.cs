using System.Buffers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using CommunityToolkit.Diagnostics;

namespace System.IO;

/// <summary>
/// Extensions for Stream
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// Enumerate Stream content by defined size chunks.
    /// </summary>
    /// <param name="stream">Stream to perform the operation on.</param>
    /// <param name="bufferSize">The size of requested chunks.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns></returns>
    public static async IAsyncEnumerable<ReadOnlyMemory<byte>> ReadChunksAsync(this Stream stream, int bufferSize = 0x10_000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(stream);

        if (bufferSize <= 0)
        {
            bufferSize = 0x10_000;
        }

        var buffer = new byte[bufferSize];
        while (true)
        {
            var readLen = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
            if (readLen == 0) // EOF
                yield break;

            if (readLen != buffer.Length)
                yield return buffer.AsMemory(0, readLen);
            else
                yield return buffer;
        }
    }

    private static HashAlgorithm CreateHashAlgorithm<THashAlgorithm>()
        where THashAlgorithm : HashAlgorithm
    {
#if NET6_0_OR_GREATER
        var createMethod = typeof(THashAlgorithm).GetMethod("Create", BindingFlags.Static | BindingFlags.Public, Array.Empty<Type>());
#else
        var createMethod = typeof(THashAlgorithm).GetMethod("Create", BindingFlags.Static | BindingFlags.Public, null, Array.Empty<Type>(), Array.Empty<ParameterModifier>());
#endif

        return (HashAlgorithm)createMethod!.Invoke(null, null)!;
    }

    /// <summary>
    /// Asynchronously reads the bytes from the source stream and writes them to destination stream. Both streams positions are advanced by the number of bytes copied.
    /// The required hash value is being calculated via requested hash algoritm type parameter and the total bytes read 
    /// </summary>
    /// <typeparam name="THashAlgorithm"></typeparam>
    /// <param name="source">Source strem where to read from.</param>
    /// <param name="destination">The destination stream to wite to.</param>
    /// <param name="totalBytesRead">Action to be called with updated total number of bytes read.</param>
    /// <param name="bufferSize">The size of requested chunks.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns></returns>
    public static async Task<string> CopyToWithProgressAndHashAsync<THashAlgorithm>(this Stream source, Stream destination, Action<long> totalBytesRead, int bufferSize = 0x10_000, CancellationToken cancellationToken = default)
        where THashAlgorithm : HashAlgorithm
    {
        Guard.IsNotNull(source);
        Guard.IsNotNull(destination);
        Guard.IsNotNull(totalBytesRead);

        if (bufferSize <= 0)
        {
            bufferSize = 0x10_000;
        }

        using var hashAlgorithm = CreateHashAlgorithm<THashAlgorithm>();
        byte[]? rentedBuffer = null;
        try
        {
            rentedBuffer = ArrayPool<byte>.Shared.Rent(0x1_000);

            var totalBytes = 0;
            int readBytes;
            do
            {
                readBytes = 0;
                while (readBytes < bufferSize)
                {
#if NETSTANDARD2_0
                    var numBytes = await source.ReadAsync(rentedBuffer, readBytes, bufferSize - readBytes, cancellationToken).ConfigureAwait(false);
#else
                    var numBytes = await source.ReadAsync(rentedBuffer.AsMemory(readBytes, bufferSize - readBytes), cancellationToken).ConfigureAwait(false);
#endif
                    if (numBytes == 0)
                    {
                        break;
                    }
                    readBytes += numBytes;
                }

#if NETSTANDARD2_0
                await destination.WriteAsync(rentedBuffer, 0, readBytes, cancellationToken).ConfigureAwait(false);
#else
                await destination.WriteAsync(rentedBuffer.AsMemory(0, readBytes), cancellationToken).ConfigureAwait(false);
#endif

                hashAlgorithm.TransformBlock(rentedBuffer, 0, readBytes, rentedBuffer, 0); // input and output buffer have to be the same

                totalBytes += readBytes;
                totalBytesRead(totalBytes);
            } while (readBytes == bufferSize);

            hashAlgorithm.TransformFinalBlock(rentedBuffer, 0, 0);

#if NETSTANDARD2_0
            return ConvertPolyfills.ToHexString(hashAlgorithm.Hash.AsReadOnlySpan());
#else
            return Convert.ToHexString(hashAlgorithm.Hash!.AsReadOnlySpan());
#endif
        }
        finally
        {
            if (rentedBuffer is not null)
            {
                ArrayPool<byte>.Shared.Return(rentedBuffer, clearArray: false);
            }
        }
    }

    /// <summary>
    /// Asynchronously computes the hash value for the specified Stream object and returns it as Hex16 encoded string.
    /// </summary>
    /// <typeparam name="THashAlgorithm">Hash algorithm.</typeparam>
    /// <param name="stream">The input to compute the hash code for.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>Base16 string encoded hash value.</returns>
    public static async Task<string> ComputeHashAsync<THashAlgorithm>(this Stream stream, CancellationToken cancellationToken = default)
        where THashAlgorithm : HashAlgorithm
    {
        Guard.IsNotNull(stream);

        using var hashAlgorithm = CreateHashAlgorithm<THashAlgorithm>();
        var hash = await hashAlgorithm.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);

#if NETSTANDARD2_0
        return ConvertPolyfills.ToHexString(hash.AsReadOnlySpan());
#else
        return Convert.ToHexString(hash.AsReadOnlySpan());
#endif
    }
}

