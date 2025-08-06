namespace CommonNet.Extensions.Tests;

public class ValueTypesExtensionsTests
{
    struct TS
    {
        public string A;
        public int B;
    }

    [Test]
    public async Task ValueTypesExtensions_IsEmptyTests()
    {
        await Assert.That(1.IsEmpty())
            .IsFalse();
        await Assert.That(int.MinValue.IsEmpty())
            .IsFalse();
        await Assert.That(int.MaxValue.IsEmpty())
            .IsFalse();
        await Assert.That(0.IsEmpty())
            .IsTrue();
        await Assert.That(true.IsEmpty())
            .IsFalse();
        await Assert.That(false.IsEmpty())
            .IsTrue();
        await Assert.That(new TS().IsEmpty())
            .IsTrue();
        await Assert.That(new TS { A = "test" }.IsEmpty())
            .IsFalse();
        await Assert.That(new Guid().IsEmpty())
            .IsTrue();
        await Assert.That(Guid.NewGuid().IsEmpty())
            .IsFalse();
        await Assert.That(new DateTime().IsEmpty())
            .IsTrue();
        await Assert.That(DateTime.MaxValue.IsEmpty())
            .IsFalse();
    }

    [Test]
    public async Task ValueTypesExtensions_IsNotEmptyTests()
    {
        await Assert.That(1.IsNotEmpty())
            .IsTrue();
        await Assert.That(int.MinValue.IsNotEmpty())
            .IsTrue();
        await Assert.That(int.MaxValue.IsNotEmpty())
            .IsTrue();
        await Assert.That(0.IsNotEmpty())
            .IsFalse();
        await Assert.That(true.IsNotEmpty())
            .IsTrue();
        await Assert.That(false.IsNotEmpty())
            .IsFalse();
        await Assert.That(new TS().IsNotEmpty())
            .IsFalse();
        await Assert.That(new TS { B = 1 }.IsNotEmpty())
            .IsTrue();
        await Assert.That(new Guid().IsNotEmpty())
            .IsFalse();
        await Assert.That(Guid.NewGuid().IsNotEmpty())
            .IsTrue();
        await Assert.That(new DateTime().IsNotEmpty())
            .IsFalse();
        await Assert.That(DateTime.MaxValue.IsNotEmpty())
            .IsTrue();
    }

    [Test]
    public async Task ValueTypesExtensions_ToNullable()
    {
        await Assert.That(((int?)1))
            .IsEqualTo(1.ToNullable());
        await Assert.That(((int?)int.MinValue))
            .IsEqualTo(int.MinValue.ToNullable());
        await Assert.That(((int?)int.MaxValue))
            .IsEqualTo(int.MaxValue.ToNullable());
        await Assert.That(0.ToNullable())
            .IsNull();
        await Assert.That(((bool?)true))
            .IsEqualTo(true.ToNullable());
        await Assert.That(false.ToNullable())
            .IsNull();
        await Assert.That(new TS().ToNullable())
            .IsNull();
        var ts = new TS { A = "test", B = 1 };
        await Assert.That(((TS?)ts))
            .IsEqualTo(ts.ToNullable());
        await Assert.That(new Guid().ToNullable())
            .IsNull();
        var guid = Guid.NewGuid();
        await Assert.That(((Guid?)guid))
            .IsEqualTo(guid.ToNullable());
        await Assert.That(new DateTime().ToNullable())
            .IsNull();
        await Assert.That(((DateTime?)DateTime.MaxValue))
            .IsEqualTo(DateTime.MaxValue.ToNullable());
    }
}
