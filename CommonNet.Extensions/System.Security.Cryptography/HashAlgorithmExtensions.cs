using System.Buffers;
using CommunityToolkit.Diagnostics;

namespace System.Security.Cryptography;

/// <summary>
/// Netstandard 2.0 polyfill HashAlgorithm extensions.
/// </summary>
public static class HashAlgorithmExtensions
{

#if NETSTANDARD2_0

    /// <summary>
    /// Netstandard 2.0 polyfill that asynchronously computes the hash value for the specified Stream object.
    /// </summary>
    /// <param name="hashAlgorithm">Hash algorithm </param>
    /// <param name="inputStream">The input to compute the hash code for.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>A task that represents the asynchronous compute hash operation and wraps the computed hash code.</returns>
    public static async Task<byte[]> ComputeHashAsync(this HashAlgorithm hashAlgorithm, Stream inputStream, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(hashAlgorithm);
        Guard.IsNotNull(inputStream);
        Guard.CanRead(inputStream);

        byte[]? rentedBuffer = null;
        var hashBuffer = new byte[hashAlgorithm.HashSize / 8];
        try
        {
            rentedBuffer = ArrayPool<byte>.Shared.Rent(0x1_000);
            int bytesRead;

            while (true)
            {
                bytesRead = await inputStream.ReadAsync(rentedBuffer, 0, rentedBuffer.Length, cancellationToken).ConfigureAwait(false);

                if (bytesRead == 0)
                {
                    // EOF
                    break;
                }

                if (bytesRead < rentedBuffer.Length)
                {
                    _ = hashAlgorithm.TransformFinalBlock(rentedBuffer, 0, bytesRead);
                    break;
                }
                else
                {
                    hashAlgorithm.TransformBlock(rentedBuffer, 0, bytesRead, rentedBuffer, 0);
                }
            }

            hashAlgorithm.Hash.AsMemory().CopyTo(hashBuffer);
            hashAlgorithm.Clear();
            return hashBuffer;
        }
        finally
        {
            if (rentedBuffer is not null)
            {
                ArrayPool<byte>.Shared.Return(rentedBuffer, clearArray: false);
            }
        }
    }

#endif

}
