namespace System
{
	using Collections.Generic;
	using CommonNet.Extensions;
	using ComponentModel;
	using Globalization;

	/// <summary>
	/// Commonly used extension methods on <see cref="string"/>.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class CommonNetStringExtensions
	{
		/// <summary>
		/// Indicates whether the specified string is not null and differs from <see cref="string.Empty"/> string.
		/// </summary>
		/// <param name="self">The string to test.</param>
		/// <returns>false if the string is null or an empty string, true otherwise.</returns>
		public static bool IsNotNullOrEmpty(this string self) => !String.IsNullOrEmpty(self);

		/// <summary>
		/// Indicates whether a specified string is not null, empty, or consists only of white-space characters.
		/// </summary>
		/// <param name="self">The string to test.</param>
		/// <returns>false if the string is null or empty string, or if consists only of white-space characters, true otherwise.</returns>
		public static bool IsNotNullOrWhiteSpace(this string self) => !String.IsNullOrWhiteSpace(self);

		// helper data to substitute TypeDescriptor.GetConverter() functionality that is not available on PCL
		private static readonly Dictionary<Type, Func<string, object>> parsers =
			new Dictionary<Type, Func<string, object>>
		{
			{ typeof(bool), (s) => bool.Parse(s) },
			{ typeof(sbyte), (s) => sbyte.Parse(s) },
			{ typeof(byte), (s) => byte.Parse(s) },
			{ typeof(char), (s) => s[0] },
			{ typeof(short), (s) => short.Parse(s) },
			{ typeof(ushort), (s) => ushort.Parse(s) },
			{ typeof(int), (s) => int.Parse(s) },
			{ typeof(uint), (s) => uint.Parse(s) },
			{ typeof(long), (s) => long.Parse(s) },
			{ typeof(ulong), (s) => ulong.Parse(s) },
			{ typeof(double), (s) => double.Parse(s, CultureInfo.InvariantCulture) },
			{ typeof(DateTime), (s) => DateTime.Parse(s, CultureInfo.InvariantCulture) },
			{ typeof(TimeSpan), (s) => TimeSpan.Parse(s, CultureInfo.InvariantCulture) },
			{ typeof(Guid), (s) => Guid.Parse(s) },
			{ typeof(Version), (s) => Version.Parse(s) },
		};

		/// <summary>
		/// Parse string into desired type.
		/// </summary>
		/// <typeparam name="T">Deduced parameter by return type or explicitly typed one.</typeparam>
		/// <param name="self">The string to parse.</param>
		/// <returns></returns>
		public static T Parse<T>(
			this string self
		)
		{
			Check.Self(self);

			var result = default(T);
			if (self.IsNotNullOrEmpty())
			{
				Func<string, object> convertFnc;
				if (!parsers.TryGetValue(typeof(T), out convertFnc))
				{
					throw new NotSupportedException();
				}
				result = (T)convertFnc(self);
			}
			return result;
		}

		/// <summary>
		/// Try parse string into desired type.
		/// </summary>
		/// <param name="self">The string to parse.</param>
		/// <param name="value">Value where to store result of parse.</param>
		/// <returns></returns>
		[Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static bool TryParse<T>(
			this string self,
			out T value
		)
		{
			try
			{
				value = self.Parse<T>();
			}
			catch
			{
				value = default(T);
				return false;
			}
			return true;
		}

	}
}
