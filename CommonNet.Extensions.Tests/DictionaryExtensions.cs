namespace Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class DictionaryExtensions
    {
        readonly Func<int, int> factoryFnc = (key) => key;
        readonly Func<int, int> factoryFncPlusOne = (key) => key + 1;
        readonly Func<int, int, int> updateFnc = (key, old) => old + key;

        [Fact]
        public void Dictionary_AddOrGetValueBasicTest()
        {
            var dict = new Dictionary<int, int>();
            var val = dict.GetOrAdd(1, 1);
            Assert.Equal(1, val);
            val = dict.GetOrAdd(1, 2);
            Assert.Equal(1, val);

            val = dict.GetOrAdd(2, factoryFnc);
            Assert.Equal(factoryFnc(2), val);
            val = dict.GetOrAdd(2, factoryFncPlusOne);
            Assert.Equal(factoryFnc(2), val);
        }

        [Fact]
        public void Dictionary_AddOrGetLazyValueBasicTest()
        {
            var dict = new Dictionary<int, Lazy<int>>();
            var val = dict.GetOrAdd(3, factoryFnc);
            Assert.Equal(factoryFnc(3), val);
            val = dict.GetOrAdd(3, factoryFncPlusOne);
            Assert.Equal(factoryFnc(3), val);
        }

        [Fact]
        public void Dictionary_AddOrUpdateValueBasicTest()
        {
            var dict = new Dictionary<int, int>();
            var val = dict.AddOrUpdate(1, 1, updateFnc);
            Assert.Equal(1, val);
            val = val = dict.AddOrUpdate(1, 1, updateFnc);
            Assert.Equal(updateFnc(1, 1), val);

            val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
            Assert.Equal(factoryFnc(2), val);
            val = val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
            Assert.Equal(updateFnc(2, 2), val);
        }

        [Fact]
        public void Dictionary_AddOrUpdateLazyValueBasicTest()
        {
            var dict = new Dictionary<int, Lazy<int>>();
            var val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
            Assert.Equal(factoryFnc(3), val);
            val = val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
            Assert.Equal(updateFnc(3, 3), val);
        }
    }
}
