namespace System.Collections.Generic
{
	using CommonNet.Extensions;
	using ComponentModel;
	using System;

	/// <summary>
	/// Commonly used extension methods on <see cref="IEnumerable{T}"/>.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class CommonNetEnumerableExtensions
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
			Check.Self(self);
			Check.Argument(action, nameof(action));

			foreach (TValue item in self)
				action(item);
		}
	}
}
