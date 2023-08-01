using System.ComponentModel;
using System.Linq;
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
    /// <param name="enumerable">Enumerable where to perform action on elements.</param>
    /// <param name="action"><see cref="Action{TValue}"/> delegate to perform on each element.</param>
    public static void ForEach<TValue>(this IEnumerable<TValue> enumerable, Action<TValue> action)
    {
        Guard.IsNotNull(enumerable);
        Guard.IsNotNull(action);

        foreach (var item in enumerable)
        {
            action(item);
        }
    }

    /// <summary>
    /// Computes the median value of a numerical sequence projected from the source sequence using the specified selector function.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
    /// <param name="enumerable">The source sequence to compute the median from.</param>
    /// <param name="selector">A selector function to project each element of the source sequence to a numerical value.</param>
    /// <returns>The median value of the numerical sequence.</returns>
    public static double Median<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, double> selector)
    {
        Guard.IsNotNull(enumerable);
        Guard.IsNotNull(selector);
        if (!enumerable.Any())
        {
            throw new ArgumentException("Cannot compute median for an empty set.", nameof(enumerable));
        }

        var sortedList = enumerable.Select(selector).OrderBy(x => x).ToList();

        var itemIndex = sortedList.Count / 2;
        if (sortedList.Count % 2 == 0)
        {
            return (sortedList[itemIndex] + sortedList[itemIndex - 1]) / 2;
        }
        else
        {
            return sortedList[itemIndex];
        }
    }

    /// <summary>
    /// Computes the median value of a sequence of double values.
    /// </summary>
    /// <param name="enumerable">The sequence of double values to compute the median from.</param>
    /// <returns>The median value of the sequence.</returns>
    public static double Median(this IEnumerable<double> enumerable) =>
        enumerable.Median(x => x);

    /// <summary>
    /// Computes the median value of a sequence of integer values.
    /// </summary>
    /// <param name="enumerable">The sequence of integer values to compute the median from.</param>
    /// <returns>The median value of the sequence.</returns>
    public static double Median(this IEnumerable<int> enumerable) =>
        enumerable.Median(x => x);
}
