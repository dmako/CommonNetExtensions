using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;

namespace System.IO;

/// <summary>
/// Commonly used extension methods on <see cref="TextReader"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class TextReaderExtensions
{
    /// <summary>
    /// Iterates over lines by using <see cref="TextReader"/> instance without writing while cycle code.
    /// </summary>
    /// <param name="readed"><see cref="TextReader"/> instance to use for retrieving lines.</param>
    /// <returns><see cref="IEnumerable{String}"/> yielded lines.</returns>
    public static IEnumerable<string> EnumLines(
        this TextReader readed
    )
    {
        Guard.IsNotNull(readed);

        string? line;
        while ((line = readed.ReadLine()) != null)
            yield return line;
    }

    /// <summary>
    /// Iterates over lines by using <see cref="TextReader"/> instance without writing while cycle code.
    /// </summary>
    /// <param name="reader"><see cref="TextReader"/> instance to use for retrieving lines.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns><see cref="IEnumerable{String}"/> yielded lines.</returns>
    public static async IAsyncEnumerable<string> EnumLinesAsync(
        this TextReader reader,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        Guard.IsNotNull(reader);

        string? line;
#if NET7_0_OR_GREATER
        while ((line = await reader.ReadLineAsync(cancellationToken)) is not null)
            yield return line!;
#else
        while ((line = await reader.ReadLineAsync()) is not null)
            yield return line!;
#endif
    }

    /// <summary>
    /// Performs <see cref="Action{String}"/> on every line while iterating over lines by using <see cref="TextReader"/> instance.
    /// </summary>
    /// <param name="readed"><see cref="TextReader"/> instance to use for retrieving lines.</param>
    /// <param name="action"><see cref="Action{String}"/> to be performed.</param>
    public static void ForEachLine(
        this TextReader readed,
        Action<string> action
    )
    {
        Guard.IsNotNull(readed);
        Guard.IsNotNull(action);

        readed.EnumLines().ForEach(line => action(line));
    }
}
