namespace CommonNet.Extensions.Tests;

public class EnumExtensionsTests
{
    public enum MyEnum
    {
        Value1,
        Value2,
        Value3,
        Value4,
    }

    [Test]
    public async Task IsOneOf_ShouldReturnTrue_WhenEnumIsInEnums()
    {
        var enums = new[] { MyEnum.Value1, MyEnum.Value2, MyEnum.Value3 };

        await Assert.That(MyEnum.Value1.IsOneOf(enums))
            .IsTrue();
        await Assert.That(MyEnum.Value2.IsOneOf(enums))
            .IsTrue();
        await Assert.That(MyEnum.Value3.IsOneOf(enums))
            .IsTrue();
    }

    [Test]
    public async Task IsOneOf_ShouldReturnFalse_WhenEnumIsNotInEnums()
    {
        var enums = new[] { MyEnum.Value4 };

        await Assert.That(MyEnum.Value1.IsOneOf(enums))
            .IsFalse();
        await Assert.That(MyEnum.Value2.IsOneOf(enums))
            .IsFalse();
        await Assert.That(MyEnum.Value3.IsOneOf(enums))
            .IsFalse();
    }

    [Test]
    public async Task IsOneOf_ShouldReturnFalse_WhenEnumsIsEmpty()
    {
        var enums = Array.Empty<MyEnum>();

        await Assert.That(MyEnum.Value1.IsOneOf(enums))
            .IsFalse();
    }

    [Test]
    public async Task IsOneOf_ShouldThrow_WhenEnumsArrayIsNull()
    {
        MyEnum[] enums = null!;

        await Assert.That(() => MyEnum.Value1.IsOneOf(enums))
            .ThrowsExactly<ArgumentNullException>();
    }
}
