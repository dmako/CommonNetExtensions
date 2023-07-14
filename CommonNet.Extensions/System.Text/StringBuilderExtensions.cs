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
    /// <param name="sb">Where to apend.</param>
    /// <param name="condition">Condition for appending decision.</param>
    /// <param name="value"></param>
    /// <returns>Returns the <paramref name="sb"/> reference.</returns>
    public static StringBuilder AppendIf(this StringBuilder sb, bool condition, string value)
    {
        Guard.IsNotNull(sb);

        if (condition)
        {
            sb.Append(value);
        }
        return sb;
    }

    /// <summary>
    /// Test the string builder whether itr ends with requested string.
    /// </summary>
    /// <param name="sb">StringBuilder object to test buffer on.</param>
    /// <param name="suffixTest">String to search at the end of buffer.</param>
    /// <param name="comparison">Type of comparision.</param>
    /// <returns>True if the suffixTest was found, false otherwise.</returns>
    public static bool EndsWith(this StringBuilder sb, string suffixTest, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        Guard.IsNotNull(sb);
        Guard.IsNotNullOrEmpty(suffixTest);

        if (sb.Length < suffixTest.Length)
            return false;

        var end = sb.ToString(sb.Length - suffixTest.Length, suffixTest.Length);
        return end.Equals(suffixTest, comparison);
    }

}
