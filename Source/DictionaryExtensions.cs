namespace System.Collections.Generic
{
	using CommonNet.Extensions;
	using ComponentModel;

	/// <summary>
	/// <see cref="Concurrent.ConcurrentDictionary{TKey, TValue}"/> does have nice API that shortens the dictionary usage.
	/// DictionaryExtensions defines the same API for <see cref="Generic.Dictionary{TKey, TValue}"/>.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class CommonNetDictionaryExtensions
	{
		/// <summary>
		/// Adds a key/value pair to the <see cref="Generic.Dictionary{TKey, TValue}"/> if the key does not already exist.
		/// </summary>
		/// <param name="self">Dictionary where to add or get value.</param>
		/// <param name="key">The key of the element to add or get.</param>
		/// <param name="valueFactory">Function that will provide value to be added, if the key does not already exist.</param>
		/// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		public static TValue GetOrAdd<TKey, TValue>(
			this Dictionary<TKey, TValue> self,
			TKey key,
			Func<TKey, TValue> valueFactory
		)
		{
			Check.Self(self);
			Check.Argument(key, nameof(key));
			Check.Argument(valueFactory, nameof(valueFactory));

			TValue value;
			if (!self.TryGetValue(key, out value))
			{
				value = valueFactory(key);
				self.Add(key, value);
			}
			return value;
		}

		/// <summary>
		/// Adds a key/value pair to the <see cref="Generic.Dictionary{TKey, TValue}"/> if the key does not already exist.
		/// </summary>
		/// <param name="self">Dictionary where to add or get value.</param>
		/// <param name="key">The key of the element to add or get.</param>
		/// <param name="newValue">The value to be added, if the key does not already exist.</param>
		/// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		public static TValue GetOrAdd<TKey, TValue>(
			this Dictionary<TKey, TValue> self,
			TKey key,
			TValue newValue
		)
		{
			Check.Self(self);
			Check.Argument(key, nameof(key));

			TValue value;
			if (!self.TryGetValue(key, out value))
			{
				value = newValue;
				self.Add(key, value);
			}
			return value;
		}

		/// <summary>
		/// Adds a key/lazy-value pair to the <see cref="Generic.Dictionary{TKey, TValue}"/> where value is <see cref="Lazy{TValue}"/> if the key does not already exist.
		/// Note that value is always initialized by this method.
		/// </summary>
		/// <param name="self">Dictionary where to add or get value.</param>
		/// <param name="key">The key of the element to add or get.</param>
		/// <param name="valueFactory">Function that will provide lazy-value, if the key does not already exist.</param>
		/// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		[Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static TValue GetOrAdd<TKey, TValue>(
			this Dictionary<TKey, Lazy<TValue>> self,
			TKey key,
			Func<TKey, TValue> valueFactory
		)
		{
			Check.Self(self);
			Check.Argument(valueFactory, nameof(valueFactory));

			return GetOrAdd<TKey, Lazy<TValue>>(self, key, new Lazy<TValue>(() => valueFactory(key), true)).Value;
		}

		/// <summary>
		/// Adds a key/value pair to the <see cref="Generic.Dictionary{TKey, TValue}"/> if the key does not already exist,
		/// or updates a key/value pair by using the specified function if the key already exists.
		/// </summary>
		/// <param name="self">Dictionary where to add or update key/value.</param>
		/// <param name="key">The key of the element to add or update.</param>
		/// <param name="addValue">The value to be added, if the key does not already exist.</param>
		/// <param name="updateValueFactory">Function that will provide value for an existing key based on the key's existing value</param>
		/// <returns>The value for the key. This will be either the updated value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		public static TValue AddOrUpdate<TKey, TValue>(
			this Dictionary<TKey, TValue> self,
			TKey key,
			TValue addValue,
			Func<TKey, TValue, TValue> updateValueFactory
		)
		{
			Check.Self(self);
			Check.Argument(key, nameof(key));
			Check.Argument(updateValueFactory, nameof(updateValueFactory));

			TValue newValue;
			TValue oldValue;
			if (self.TryGetValue(key, out oldValue))
			{
				newValue = updateValueFactory(key, oldValue);
				self[key] = newValue;
			}
			else
			{
				newValue = addValue;
				self.Add(key, newValue);
			}
			return newValue;
		}

		/// <summary>
		/// Adds a key/value pair to the <see cref="Generic.Dictionary{TKey, TValue}"/> if the key does not already exist,
		/// or updates a key/value pair by using the specified function if the key already exists.
		/// </summary>
		/// <param name="self">Dictionary where to add or update key/value.</param>
		/// <param name="key">The key of the element to add or update.</param>
		/// <param name="addValueFactory">Function that will provide value to be added, if the key does not already exist.</param>
		/// <param name="updateValueFactory">Function that will provide value for an existing key based on the key's existing value</param>
		/// <returns>The value for the key. This will be either the updated value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		public static TValue AddOrUpdate<TKey, TValue>(
			this Dictionary<TKey, TValue> self,
			TKey key,
			Func<TKey, TValue> addValueFactory,
			Func<TKey, TValue, TValue> updateValueFactory
		)
		{
			Check.Self(self);
			Check.Argument(key, nameof(key));
			Check.Argument(addValueFactory, nameof(addValueFactory));
			Check.Argument(updateValueFactory, nameof(updateValueFactory));

			TValue newValue;
			TValue oldValue;
			if (self.TryGetValue(key, out oldValue))
			{
				newValue = updateValueFactory(key, oldValue);
				self[key] = newValue;
			}
			else
			{
				newValue = addValueFactory(key);
				self.Add(key, newValue);
			}
			return newValue;
		}

		/// <summary>
		/// Adds a key/lazy-value pair to the <see cref="Generic.Dictionary{TKey, TValue}"/> if the key does not already exist,
		/// or updates a key/lazy-value pair by using the specified function if the key already exists.
		/// Note that value is always initialized by this method.
		/// </summary>
		/// <param name="self">Dictionary where to add or update key/value.</param>
		/// <param name="key">The key of the element to add or update.</param>
		/// <param name="addValueFactory">Function that will provide value to be added, if the key does not already exist.</param>
		/// <param name="updateValueFactory">Function that will provide value for an existing key based on the key's existing value</param>
		/// <returns>The value for the key. This will be either the updated value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
		[Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static TValue AddOrUpdate<TKey, TValue>(
			this Dictionary<TKey, Lazy<TValue>> self,
			TKey key,
			Func<TKey, TValue> addValueFactory,
			Func<TKey, TValue, TValue> updateValueFactory
		)
		{
			Check.Self(self);
			Check.Argument(addValueFactory, nameof(addValueFactory));
			Check.Argument(updateValueFactory, nameof(updateValueFactory));

			return AddOrUpdate(self, key, new Lazy<TValue>(() => addValueFactory(key), true), (k, v) => new Lazy<TValue>(() => updateValueFactory(k, v.Value), true)).Value;
		}
	}
}
