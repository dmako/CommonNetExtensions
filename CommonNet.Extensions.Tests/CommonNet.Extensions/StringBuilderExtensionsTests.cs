using System.Text;
using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendIf_ShouldThrow_WhenNullStringBuilderIsGiven()
    {
        StringBuilder sb = null!;
        var fnc = () => sb.AppendIf(true, "null");
        fnc.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void AppendIf_ShouldAppendValue_WhenConditionIsTrue()
    {
        var sb = new StringBuilder("Hello, ");
        var condition = true;
        var valueToAppend = "World!";

        sb.AppendIf(condition, valueToAppend);

        sb.ToString().Should().Be("Hello, World!");
    }

    [Fact]
    public void AppendIf_ShouldNotAppendValue_WhenConditionIsFalse()
    {
        var sb = new StringBuilder("Hello, ");
        var condition = false;
        var valueToAppend = "World!";

        sb.AppendIf(condition, valueToAppend);

        sb.ToString().Should().Be("Hello, ");
    }

    [Fact]
    public void AppendIf_ShouldThrow_WhenInvalidValueIsGiven()
    {
        var sb = new StringBuilder();
        var fnc = () => sb.AppendIf(true, null!);
        fnc.Should().ThrowExactly<ArgumentNullException>();

        fnc = () => sb.AppendIf(true, string.Empty);
        fnc.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AppendIf_TypeConstructExampleShouldSucceed()
    {
        var sb = new StringBuilder();
        var type = typeof(StringBuilderExtensionsTests);

        sb.AppendIf(type.IsNotPublic, "internal")
            .AppendIf(type.IsPublic, "public")
            .Append(' ')
            .AppendIf(type.IsClass, "class")
            .AppendIf(type.IsByRef, "struct")
            .Append(' ')
            .Append(type.Name);

        var result = sb.ToString();
        result.Should().Be($"public class {nameof(StringBuilderExtensionsTests)}");
    }


    [Fact]
    public void EndsWith_ShouldReturnTrue_WhenSuffixIsPresent()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "World!";
        var result = sb.EndsWith(suffix);
        result.Should().BeTrue();

        sb.Clear().Append("Hello\r\n");
        suffix = "\r\n";
        result = sb.EndsWith(suffix);
        result.Should().BeTrue();
    }

    [Fact]
    public void EndsWith_ShouldReturnFalse_WhenSuffixIsNotPresent()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "Universe!";
        var result = sb.EndsWith(suffix);
        result.Should().BeFalse();
    }

    [Fact]
    public void EndsWith_ShouldReturnTrue_WhenSuffixIsPresentWithCaseInsensitiveComparison()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "world!";
        var result = sb.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase);
        result.Should().BeTrue();
    }

    [Fact]
    public void EndsWith_ShouldReturnFalse_WhenSuffixIsNotPresentWithCaseInsensitiveComparison()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "universe!";
        var result = sb.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase);
        result.Should().BeFalse();
    }

    [Fact]
    public void EndsWith_ShouldThrow_WhenNullStringBuilderIsGiven()
    {
        StringBuilder sb = null!;
        var func = () => sb.EndsWith("World!");
        func.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void EndsWith_ShouldThrow_WhenInvalidSuffixValueIsGiven()
    {
        var sb = new StringBuilder("Hello, World!");
        var func = () => sb.EndsWith(string.Empty);
        func.Should().ThrowExactly<ArgumentException>();
        func = () => sb.EndsWith(null!);
        func.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void EndsWith_ShouldReturnFalse_WhenSuffixIsLongerThanStringBuilder()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "ThisIsALongSuffix";
        var result = sb.EndsWith(suffix);
        result.Should().BeFalse();
    }
}
