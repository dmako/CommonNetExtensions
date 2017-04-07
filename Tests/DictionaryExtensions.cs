namespace Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class DictionaryExtensions
    {
        readonly Func<int, int> factoryFnc = (key) => key;
        readonly Func<int, int> factoryFncPlusOne = (key) => key + 1;
        readonly Func<int, int, int> updateFnc = (key, old) => old + key;

        [Test]
        public void Dictionary_AddOrGetValueBasicTest()
        {
            var dict = new Dictionary<int, int>();
            var val = dict.GetOrAdd(1, 1);
            Assert.AreEqual(1, val);
            val = dict.GetOrAdd(1, 2);
            Assert.AreEqual(1, val);

            val = dict.GetOrAdd(2, factoryFnc);
            Assert.AreEqual(factoryFnc(2), val);
            val = dict.GetOrAdd(2, factoryFncPlusOne);
            Assert.AreEqual(factoryFnc(2), val);
        }

        [Test]
        public void Dictionary_AddOrGetLazyValueBasicTest()
        {
            var dict = new Dictionary<int, Lazy<int>>();
            var val = dict.GetOrAdd(3, factoryFnc);
            Assert.AreEqual(factoryFnc(3), val);
            val = dict.GetOrAdd(3, factoryFncPlusOne);
            Assert.AreEqual(factoryFnc(3), val);
        }

        [Test]
        public void Dictionary_AddOrUpdateValueBasicTest()
        {
            var dict = new Dictionary<int, int>();
            var val = dict.AddOrUpdate(1, 1, updateFnc);
            Assert.AreEqual(1, val);
            val = val = dict.AddOrUpdate(1, 1, updateFnc);
            Assert.AreEqual(updateFnc(1, 1), val);

            val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
            Assert.AreEqual(factoryFnc(2), val);
            val = val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
            Assert.AreEqual(updateFnc(2, 2), val);
        }

        [Test]
        public void Dictionary_AddOrUpdateLazyValueBasicTest()
        {
            var dict = new Dictionary<int, Lazy<int>>();
            var val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
            Assert.AreEqual(factoryFnc(3), val);
            val = val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
            Assert.AreEqual(updateFnc(3, 3), val);
        }
    }
}
