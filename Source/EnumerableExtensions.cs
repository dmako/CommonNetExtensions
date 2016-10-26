namespace System.Collections.Generic
{
	using CommonNet.Extensions;
	using System;

	public static class EnumerableExtensions
	{
		/// <summary>
		///   Performs action on each element of the <see cref="IEnumerable{TValue}"/>.
		///   This function allows using the same expressions like when using <see cref="List{TValue}.ForEach(Action{TValue})"/>.
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
