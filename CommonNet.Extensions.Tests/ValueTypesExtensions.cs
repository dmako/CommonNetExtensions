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
