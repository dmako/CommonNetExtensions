namespace Tests
{
    using Xunit;
    using System;

    public class ValueTypesExtensions
    {
        struct TS
        {
            public string A;
            public int B;
        }

        [Fact]
        public void ValueTypesExtensions_IsEmptyTests()
        {
            Assert.False(1.IsEmpty());
            Assert.False(int.MinValue.IsEmpty());
            Assert.False(int.MaxValue.IsEmpty());
            Assert.True(0.IsEmpty());
            Assert.False(true.IsEmpty());
            Assert.True(false.IsEmpty());
            Assert.True(new TS().IsEmpty());
            Assert.False(new TS { A = "test" }.IsEmpty());
            Assert.True(new Guid().IsEmpty());
            Assert.False(Guid.NewGuid().IsEmpty());
            Assert.True(new DateTime().IsEmpty());
            Assert.False(DateTime.MaxValue.IsEmpty());
        }

        [Fact]
        public void ValueTypesExtensions_IsNotEmptyTests()
        {
            Assert.True(1.IsNotEmpty());
            Assert.True(int.MinValue.IsNotEmpty());
            Assert.True(int.MaxValue.IsNotEmpty());
            Assert.False(0.IsNotEmpty());
            Assert.True(true.IsNotEmpty());
            Assert.False(false.IsNotEmpty());
            Assert.False(new TS().IsNotEmpty());
            Assert.True(new TS { B = 1 }.IsNotEmpty());
            Assert.False(new Guid().IsNotEmpty());
            Assert.True(Guid.NewGuid().IsNotEmpty());
            Assert.False(new DateTime().IsNotEmpty());
            Assert.True(DateTime.MaxValue.IsNotEmpty());
        }

        [Fact]
        public void ValueTypesExtensions_ToNullable()
        {
            Assert.Equal((int?)1, 1.ToNullable());
            Assert.Equal((int?)int.MinValue, int.MinValue.ToNullable());
            Assert.Equal((int?)int.MaxValue, int.MaxValue.ToNullable());
            Assert.Null(0.ToNullable());
            Assert.Equal((bool?)true, true.ToNullable());
            Assert.Null(false.ToNullable());
            Assert.Null(new TS().ToNullable());
            var ts = new TS { A = "test", B = 1 };
            Assert.Equal((TS?)ts, ts.ToNullable());
            Assert.Null(new Guid().ToNullable());
            var guid = Guid.NewGuid();
            Assert.Equal((Guid?)guid, guid.ToNullable());
            Assert.Null(new DateTime().ToNullable());
            Assert.Equal((DateTime?)DateTime.MaxValue, DateTime.MaxValue.ToNullable());
        }
    }
}
