namespace Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ValueTypesExtensions
    {
        struct TS
        {
            public string A;
            public int B;
        }

        [Test]
        public void ValueTypesExtensions_IsEmptyTests()
        {
            Assert.AreEqual(1.IsEmpty(), false);
            Assert.AreEqual(int.MinValue.IsEmpty(), false);
            Assert.AreEqual(int.MaxValue.IsEmpty(), false);
            Assert.AreEqual(0.IsEmpty(), true);
            Assert.AreEqual(true.IsEmpty(), false);
            Assert.AreEqual(false.IsEmpty(), true);
            Assert.AreEqual(new TS().IsEmpty(), true);
            Assert.AreEqual(new TS { A = "test" }.IsEmpty(), false);
            Assert.AreEqual(new Guid().IsEmpty(), true);
            Assert.AreEqual(Guid.NewGuid().IsEmpty(), false);
            Assert.AreEqual(new DateTime().IsEmpty(), true);
            Assert.AreEqual(DateTime.MaxValue.IsEmpty(), false);
        }

        [Test]
        public void ValueTypesExtensions_IsNotEmptyTests()
        {
            Assert.AreEqual(1.IsNotEmpty(), true);
            Assert.AreEqual(int.MinValue.IsNotEmpty(), true);
            Assert.AreEqual(int.MaxValue.IsNotEmpty(), true);
            Assert.AreEqual(0.IsNotEmpty(), false);
            Assert.AreEqual(true.IsNotEmpty(), true);
            Assert.AreEqual(false.IsNotEmpty(), false);
            Assert.AreEqual(new TS().IsNotEmpty(), false);
            Assert.AreEqual(new TS { B = 1 }.IsNotEmpty(), true);
            Assert.AreEqual(new Guid().IsNotEmpty(), false);
            Assert.AreEqual(Guid.NewGuid().IsNotEmpty(), true);
            Assert.AreEqual(new DateTime().IsNotEmpty(), false);
            Assert.AreEqual(DateTime.MaxValue.IsNotEmpty(), true);
        }

        [Test]
        public void ValueTypesExtensions_ToNullable()
        {
            Assert.AreEqual(1.ToNullable(), (int?)1);
            Assert.AreEqual(int.MinValue.ToNullable(), (int?)int.MinValue);
            Assert.AreEqual(int.MaxValue.ToNullable(), (int?)int.MaxValue);
            Assert.AreEqual(0.ToNullable(), null);
            Assert.AreEqual(true.ToNullable(), (bool?)true);
            Assert.AreEqual(false.ToNullable(), null);
            Assert.AreEqual(new TS().ToNullable(), null);
            var ts = new TS { A = "test", B = 1 };
            Assert.AreEqual(ts.ToNullable(), (TS?)ts);
            Assert.AreEqual(new Guid().ToNullable(), null);
            var guid = Guid.NewGuid();
            Assert.AreEqual(guid.ToNullable(), (Guid?)guid);
            Assert.AreEqual(new DateTime().ToNullable(), null);
            Assert.AreEqual(DateTime.MaxValue.ToNullable(), (DateTime?)DateTime.MaxValue);
        }
    }
}
