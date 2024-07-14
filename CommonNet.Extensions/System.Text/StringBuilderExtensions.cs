using System.ComponentModel;
using CommunityToolkit.Diagnostics;

namespace System.Text;

/// <summary>
/// Commonly used extension methods on <see cref="StringBuilder"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class StringBuilderExtensions
{
    /// <summary>
    /// Conditionally append string value to a <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="sb">Where to append.</param>
    /// <param name="condition">Condition for appending decision.</param>
    /// <param name="value">The string to append.</param>
    /// <returns>Returns the <paramref name="sb"/> reference.</returns>
    public static StringBuilder AppendIf(this StringBuilder sb, bool condition, string value)
    {
        Guard.IsNotNull(sb);
        Guard.IsNotNullOrEmpty(value);

        if (condition)
        {
            _ = sb.Append(value);
        }
        return sb;
    }

    /// <summary>
    /// Conditionally append string value to a <see cref="StringBuilder"/> with line ending.
    /// </summary>
    /// <param name="sb">Where to append.</param>
    /// <param name="condition">Condition for appending decision.</param>
    /// <param name="value">The string to append.</param>
    /// <returns>Returns the <paramref name="sb"/> reference.</returns>
    public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, string value)
    {
        Guard.IsNotNull(sb);
        Guard.IsNotNull(value);

        if (condition)
        {
            _ = sb.AppendLine(value);
        }
        return sb;
    }

    /// <summary>
    /// Appends a collection of strings to the <see cref="StringBuilder"/>, each string followed by a line terminator.
    /// </summary>
    /// <param name="sb">The <see cref="StringBuilder"/> to append to.</param>
    /// <param name="lines">The collection of strings to append.</param>
    /// <returns>The <see cref="StringBuilder"/> instance after appending the given lines.</returns>
    public static StringBuilder AppendLines(this StringBuilder sb, IEnumerable<string> lines)
    {
        Guard.IsNotNull(sb);
        Guard.IsNotNull(lines);

        foreach (var line in lines)
        {
            _ = sb.AppendLine(line);
        }
        return sb;
    }

    /// <summary>
    /// Test the string builder whether itr ends with requested string.
    /// </summary>
    /// <param name="sb">StringBuilder object to test buffer on.</param>
    /// <param name="suffixTest">String to search at the end of buffer.</param>
    /// <param name="comparison">Type of comparison.</param>
    /// <returns>True if the suffixTest was found, false otherwise.</returns>
    public static bool EndsWith(this StringBuilder sb, string suffixTest, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        Guard.IsNotNull(sb);
        Guard.IsNotNullOrEmpty(suffixTest);

        if (sb.Length < suffixTest.Length)
        {
            return false;
        }

        var end = sb.ToString(sb.Length - suffixTest.Length, suffixTest.Length);
        return end.Equals(suffixTest, comparison);
    }

}
