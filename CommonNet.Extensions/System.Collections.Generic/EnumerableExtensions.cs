using System.ComponentModel;
using CommunityToolkit.Diagnostics;

namespace System.Collections.Generic;

/// <summary>
/// Commonly used extension methods on <see cref="IEnumerable{T}"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class EnumerableExtensions
{
    /// <summary>
    /// Performs action on each element of the <see cref="IEnumerable{TValue}"/>.
    /// This function allows using the same expressions like when using <see cref="List{TValue}.ForEach(Action{TValue})"/>.
    /// </summary>
    /// <param name="self">Enumerable where to perform action on elements.</param>
    /// <param name="action"><see cref="Action{TValue}"/> delegate to perform on each element.</param>
    public static void ForEach<TValue>(
        this IEnumerable<TValue> self,
        Action<TValue> action
    )
    {
        Guard.IsNotNull(self);
        Guard.IsNotNull(action);

        foreach (var item in self)
            action(item);
    }
}
