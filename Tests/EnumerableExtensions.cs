namespace Tests
{
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	[TestFixture]
	public class EnumerableExtensions
	{

		[Test]
		public void Enumerable_ForEachBasicTest()
		{

			var data = new object[] {};
			Assert.Catch<ArgumentNullException>(() => ((object[])null).ForEach(o => { }));
			Assert.Catch<ArgumentNullException>(() => data.ForEach(null));
		}

		[FsCheck.NUnit.Property(MaxTest = 100, Description = nameof(Enumerable_PropertyForEachTest), QuietOnSuccess = true)]
		public void Enumerable_PropertyForEachTest(int[] data)
		{
			var sum = 0;
			data.ForEach(v => sum += v);
			Assert.AreEqual(sum, data.Sum());
		}
	}
}
