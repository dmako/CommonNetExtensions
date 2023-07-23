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

#if NET6_0_OR_GREATER
        var createMethod = typeof(THashAlgorithm).GetMethod("Create", BindingFlags.Static | BindingFlags.Public, Array.Empty<Type>());
#else
        var createMethod = typeof(THashAlgorithm).GetMethod("Create", BindingFlags.Static | BindingFlags.Public, null, Array.Empty<Type>(), Array.Empty<ParameterModifier>());
#endif

        using var hash = (HashAlgorithm)createMethod!.Invoke(null, null)!;
        var buffer = new byte[bufferSize];
        var totalBytes = 0;
        int readBytes;
        do
        {
            readBytes = 0;
            while (readBytes < bufferSize)
            {
                var numBytes = await source.ReadAsync(buffer, readBytes, bufferSize - readBytes, cancellationToken);
                if (numBytes == 0)
                {
                    break;
                }
                readBytes += numBytes;
            }

            await destination.WriteAsync(buffer, 0, readBytes, cancellationToken);

            hash.TransformBlock(buffer, 0, readBytes, buffer, 0); // input and output buffer have to be the same

            totalBytes += readBytes;
            totalBytesRead(totalBytes);
        } while (readBytes == bufferSize);

        hash.TransformFinalBlock(buffer, 0, 0);

#if NETSTANDARD2_0
        return ConvertPolyfills.ToHexString(hash.Hash.AsReadOnlySpan());
#else
        return Convert.ToHexString(hash.Hash!.AsReadOnlySpan());
#endif
    }


}

