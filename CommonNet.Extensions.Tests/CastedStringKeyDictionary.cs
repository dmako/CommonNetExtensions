using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CommonNet.Extensions.Tests;

public class CastedStringKeyDictionary(IDictionary sourceDictionary) : IDictionary<string, object?>
{
    private readonly IDictionary _nestedDictionary = sourceDictionary;

    public ICollection<string> Keys => _nestedDictionary.Keys.Cast<string>().ToList();

    public ICollection<object?> Values => _nestedDictionary.Values.Cast<object?>().ToList();

    public int Count => _nestedDictionary.Count;

    public bool IsReadOnly => _nestedDictionary.IsReadOnly;

    public object? this[string key]
    {
        get => _nestedDictionary[key];
        set => _nestedDictionary[key] = value;
    }

    public void Add(string key, object? value) => _nestedDictionary.Add(key, value);

    public bool ContainsKey(string key) => _nestedDictionary.Contains(key);

    public bool Remove(string key)
    {
        if (_nestedDictionary.Contains(key))
        {
            _nestedDictionary.Remove(key);
            return true;
        }
        return false;
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        if (_nestedDictionary.Contains(key))
        {
            value = _nestedDictionary[key];
            return true;
        }
        value = null;
        return false;
    }

    public void Add(KeyValuePair<string, object?> item) => _nestedDictionary.Add(item.Key, item.Value);

    public void Clear() => _nestedDictionary.Clear();

    public bool Contains(KeyValuePair<string, object?> item) =>
        _nestedDictionary.Contains(item.Key) && EqualityComparer<object?>.Default.Equals(this[item.Key], item.Value);

    public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) => _nestedDictionary.CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<string, object?> item)
    {
        if (Contains(item))
        {
            _nestedDictionary.Remove(item.Key);
            return true;
        }

        return false;
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
        _nestedDictionary.Cast<KeyValuePair<string, object?>>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _nestedDictionary.GetEnumerator();
}
