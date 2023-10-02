using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests.CommonNet.Extensions;

public class EnumExtensionsTests
{
    public enum MyEnum
    {
        Value1,
        Value2,
        Value3,
        Value4,
    }

    [Fact]
    public void IsOneOf_ShouldReturnTrue_WhenEnumIsInEnums()
    {
        var enums = new[] { MyEnum.Value1, MyEnum.Value2, MyEnum.Value3 };

        MyEnum.Value1.IsOneOf(enums).Should().BeTrue();
        MyEnum.Value2.IsOneOf(enums).Should().BeTrue();
        MyEnum.Value3.IsOneOf(enums).Should().BeTrue();
    }

    [Fact]
    public void IsOneOf_ShouldReturnFalse_WhenEnumIsNotInEnums()
    {
        var enums = new[] { MyEnum.Value4 };

        MyEnum.Value1.IsOneOf(enums).Should().BeFalse();
        MyEnum.Value2.IsOneOf(enums).Should().BeFalse();
        MyEnum.Value3.IsOneOf(enums).Should().BeFalse();
    }

    [Fact]
    public void IsOneOf_ShouldReturnFalse_WhenEnumsIsEmpty()
    {
        var enums = Array.Empty<MyEnum>();

        MyEnum.Value1.IsOneOf(enums).Should().BeFalse();
    }

    [Fact]
    public void IsOneOf_ShouldThrow_WhenEnumsArrayIsNull()
    {
        MyEnum[] enums = null!;

        var fnc = () => MyEnum.Value1.IsOneOf(enums);
        fnc.Should().ThrowExactly<ArgumentNullException>();
    }
}
