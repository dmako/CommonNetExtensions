using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

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
        1.IsEmpty().Should().BeFalse();
        int.MinValue.IsEmpty().Should().BeFalse();
        int.MaxValue.IsEmpty().Should().BeFalse();
        0.IsEmpty().Should().BeTrue();
        true.IsEmpty().Should().BeFalse();
        false.IsEmpty().Should().BeTrue();
        new TS().IsEmpty().Should().BeTrue();
        new TS { A = "test" }.IsEmpty().Should().BeFalse();
        new Guid().IsEmpty().Should().BeTrue();
        Guid.NewGuid().IsEmpty().Should().BeFalse();
        new DateTime().IsEmpty().Should().BeTrue();
        DateTime.MaxValue.IsEmpty().Should().BeFalse();
    }

    [Fact]
    public void ValueTypesExtensions_IsNotEmptyTests()
    {
        1.IsNotEmpty().Should().BeTrue();
        int.MinValue.IsNotEmpty().Should().BeTrue();
        int.MaxValue.IsNotEmpty().Should().BeTrue();
        0.IsNotEmpty().Should().BeFalse();
        true.IsNotEmpty().Should().BeTrue();
        false.IsNotEmpty().Should().BeFalse();
        new TS().IsNotEmpty().Should().BeFalse();
        new TS { B = 1 }.IsNotEmpty().Should().BeTrue();
        new Guid().IsNotEmpty().Should().BeFalse();
        Guid.NewGuid().IsNotEmpty().Should().BeTrue();
        new DateTime().IsNotEmpty().Should().BeFalse();
        DateTime.MaxValue.IsNotEmpty().Should().BeTrue();
    }

    [Fact]
    public void ValueTypesExtensions_ToNullable()
    {
        ((int?)1).Should().Be(1.ToNullable());
        ((int?)int.MinValue).Should().Be(int.MinValue.ToNullable());
        ((int?)int.MaxValue).Should().Be(int.MaxValue.ToNullable());
        0.ToNullable().Should().BeNull();
        ((bool?)true).Should().Be(true.ToNullable());
        false.ToNullable().Should().BeNull();
        new TS().ToNullable().Should().BeNull();
        var ts = new TS { A = "test", B = 1 };
        ((TS?)ts).Should().Be(ts.ToNullable());
        new Guid().ToNullable().Should().BeNull();
        var guid = Guid.NewGuid();
        ((Guid?)guid).Should().Be(guid.ToNullable());
        new DateTime().ToNullable().Should().BeNull();
        ((DateTime?)DateTime.MaxValue).Should().Be(DateTime.MaxValue.ToNullable());
    }
}
