#if NETSTANDARD2_0

using CommunityToolkit.Diagnostics;

namespace System.IO;

/// <summary>
/// Netstandard 2.0 Stream asynchronous polyfills
/// </summary>
public static class StreamPolyfillsExtensions
{
    /// <summary>
    /// Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.
    /// </summary>
    /// <param name="stream">Stream to perform the operation on.</param>
    /// <param name="buffer">The array to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>
    /// A task that represents the asynchronous read operation. The value of its Result property contains the total number of bytes read into the buffer.
    /// The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the stream has been reached.
    /// </returns>
    public static ValueTask<int> ReadAsync(this Stream stream, byte[] buffer, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(stream);

        try
        {
            return new ValueTask<int>(stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken));
        }
        catch (Exception ex)
        {
            return new ValueTask<int>(Task.FromException<int>(ex));
        }
    }

    /// <summary>
    /// Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">Stream to perform the operation on.</param>
    /// <param name="buffer">The array to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static ValueTask WriteAsync(this Stream stream, byte[] buffer, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(stream);

        try
        {
            return new ValueTask(stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken));
        }
        catch (Exception ex)
        {
            return new ValueTask(Task.FromException(ex));
        }
    }

    /// <summary>
    /// Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">Stream to perform the operation on.</param>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static ValueTask WriteAsync(this Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(stream);

        var data = buffer.ToArray();
        return stream.WriteAsync(data, cancellationToken);
    }
}

#endif
