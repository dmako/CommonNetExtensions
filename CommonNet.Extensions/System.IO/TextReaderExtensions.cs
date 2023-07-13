using System.ComponentModel;
using CommunityToolkit.Diagnostics;

namespace System.IO;

/// <summary>
/// Commonly used extension methods on <see cref="TextReader"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class sTextReaderExtensions
{
    /// <summary>
    /// Iterates over lines by using <see cref="TextReader"/> instance without writing while cycle code.
    /// </summary>
    /// <param name="self"><see cref="TextReader"/> instance to use for retrieving lines.</param>
    /// <returns><see cref="IEnumerable{String}"/> yielded lines.</returns>
    public static IEnumerable<string> EnumLines(
        this TextReader self
    )
    {
        Guard.IsNotNull(self);

        string? line;
        while ((line = self.ReadLine()) != null)
            yield return line;
    }

    /// <summary>
    /// Performs <see cref="Action{String}"/> on every line while iterating over lines by using <see cref="TextReader"/> instance.
    /// </summary>
    /// <param name="self"><see cref="TextReader"/> instance to use for retrieving lines.</param>
    /// <param name="action"><see cref="Action{String}"/> to be performed.</param>
    public static void ForEachLine(
        this TextReader self,
        Action<string> action
    )
    {
        Guard.IsNotNull(self);
        Guard.IsNotNull(action);

        self.EnumLines().ForEach(line => action(line));
    }
}
