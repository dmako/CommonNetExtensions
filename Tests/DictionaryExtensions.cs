namespace Tests
{
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;

	[TestFixture]
	public class DictionaryExtensions
	{
		[Test]
		public void DictionaryExtensions_AddOrGetValueBasicTest()
		{
			var dict = new Dictionary<int, int>();
			var val = dict.GetOrAdd(1, 1);
			Assert.AreEqual(1, val);
			val = dict.GetOrAdd(1, 2);
			Assert.AreEqual(1, val);

			val = dict.GetOrAdd(2, (key) => 1);
			Assert.AreEqual(1, val);
			val = dict.GetOrAdd(2, (key) => 2);
			Assert.AreEqual(1, val);
		}

		[Test]
		public void DictionaryExtensions_AddOrGetLazyValueBasicTest()
		{
			var dict = new Dictionary<int, Lazy<int>>();
			var val = dict.GetOrAdd(1, (key) => 1);
			Assert.AreEqual(1, val);
			val = dict.GetOrAdd(1, (key) => 2);
			Assert.AreEqual(1, val);
		}
	}
}
