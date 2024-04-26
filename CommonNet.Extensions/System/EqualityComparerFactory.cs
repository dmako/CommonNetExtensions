using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// Provides methods to create different types of equality comparers.
/// </summary>
public static class EqualityComparerFactory
{
    /// <summary>
    /// Creates an equality comparer.
    /// </summary>
    /// <typeparam name="T">The type used for comparison.</typeparam>
    /// <param name="equals">Function defining equality between two instances.</param>
    /// <param name="getHashCode">Function to compute the hash code (optional). If not provided the GetHashCode methods throws NotSupportedException, but it's not needed for most scenarios.</param>
    /// <returns>An equality comparer for the specified type.</returns>
    public static IEqualityComparer<T> Create<T>(
        Func<T?, T?, bool> equals,
        Func<T, int>? getHashCode = null
    )
    {
        Guard.IsNotNull(equals);
        return new EqualityComparer<T>(equals, getHashCode);
    }

    /// <summary>
    /// Creates a keyed equality comparer for reference types.
    /// </summary>
    /// <typeparam name="T">The type of the reference type.</typeparam>
    /// <typeparam name="TKey">The type of the key used for comparison.</typeparam>
    /// <param name="keySelector">Function to extract the key from the reference type.</param>
    /// <param name="equals">Function defining equality between two keys (optional). If not provided it tries to compare keys using their default equality if TKey implements IEquatable{TKey}, throws NotSupportedException otherwise.</param>
    /// <param name="getHashCode">Function to compute the hash code for keys (optional). If not provided the GetHashCode methods throws NotSupportedException, but it's not needed for most scenarios.</param>
    /// <returns>A keyed equality comparer for the specified reference type.</returns>
    public static IEqualityComparer<T> CreateKeyed<T, TKey>(
        Func<T, TKey> keySelector,
        Func<TKey?, TKey?, bool>? equals = null,
        Func<TKey, int>? getHashCode = null
    )
    {
        Guard.IsNotNull(keySelector);
        return new KeyEqualityComparer<T, TKey>(keySelector, equals, getHashCode);
    }


    private sealed class EqualityComparer<T>(Func<T?, T?, bool> equals, Func<T, int>? getHashCode) : IEqualityComparer<T>
    {
        private readonly Func<T?, T?, bool> _equals = equals;
        private readonly Func<T, int>? _getHashCode = getHashCode;

        public bool Equals(T? x, T? y) => _equals(x, y);

        public int GetHashCode(T obj) => _getHashCode is not null ? _getHashCode(obj) : throw new NotSupportedException();
    }

    private sealed class KeyEqualityComparer<T, TKey>(Func<T, TKey> keySelector, Func<TKey?, TKey?, bool>? equals, Func<TKey, int>? getHashCode) : IEqualityComparer<T>
    {
        private readonly Func<T, TKey> _keySelector = keySelector;
        private readonly Func<TKey, int>? _getHashCode = getHashCode;
        private readonly Func<TKey?, TKey?, bool>? _equals = equals;

        public bool Equals(T? x, T? y)
        {
            if (_equals is not null)
            {
                return _equals(_keySelector(x!), _keySelector(y!));
            }
            else
            {
                var xKey = _keySelector(x!);
                if (xKey is IEquatable<TKey>)
                {
                    return xKey.Equals(_keySelector(y!));
                }
            }
            throw new NotSupportedException();
        }

        public int GetHashCode(T obj) => _getHashCode is not null ? _getHashCode(_keySelector(obj)) : throw new NotSupportedException();
    }
}
