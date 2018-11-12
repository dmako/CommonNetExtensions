namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class EnumerableExtensions
    {
        [Fact]
        public void Enumerable_ForEachBasicTest()
        {
            var data = new object[] { };
            Assert.Throws<ArgumentNullException>(() => ((object[])null).ForEach(o => { }));
            Assert.Throws<ArgumentNullException>(() => data.ForEach(null));
        }

        [FsCheck.Xunit.Property(MaxTest = 100, DisplayName = nameof(Enumerable_PropertyForEachTest), QuietOnSuccess = true)]
        public void Enumerable_PropertyForEachTest(int[] data)
        {
            var sum = 0;
            data.ForEach(v => sum += v);
            Assert.Equal(sum, data.Sum());
        }
    }
}
