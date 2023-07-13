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
    /// <param name="self">Where to apend.</param>
    /// <param name="condition">Condition for appending decision.</param>
    /// <param name="value"></param>
    /// <returns>Returns the <paramref name="self"/> reference.</returns>
    public static StringBuilder AppendIf(this StringBuilder self, bool condition, string value)
    {
        Guard.IsNotNull(self);

        if (condition)
        {
            self.Append(value);
        }
        return self;
    }
}
