using System.Text;

namespace CommonNet.Extensions.Tests;

public class StringBuilderExtensionsTests
{
    [Test]
    public async Task AppendIf_ShouldThrow_WhenNullStringBuilderIsGiven()
    {
        StringBuilder sb = null!;
        await Assert.That(() => sb.AppendIf(true, "null"))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task AppendIf_ShouldAppendValue_WhenConditionIsTrue()
    {
        var sb = new StringBuilder("Hello, ");
        var condition = true;
        var valueToAppend = "World!";

        sb.AppendIf(condition, valueToAppend);
        await Assert.That(sb.ToString())
            .IsEqualTo("Hello, World!");
    }

    [Test]
    public async Task AppendIf_ShouldNotAppendValue_WhenConditionIsFalse()
    {
        var sb = new StringBuilder("Hello, ");
        var condition = false;
        var valueToAppend = "World!";

        sb.AppendIf(condition, valueToAppend);
        await Assert.That(sb.ToString())
            .IsEqualTo("Hello, ");
    }

    [Test]
    public async Task AppendIf_ShouldThrow_WhenInvalidValueIsGiven()
    {
        var sb = new StringBuilder();
        await Assert.That(() => sb.AppendIf(true, null!))
            .ThrowsExactly<ArgumentNullException>();

        await Assert.That(() => sb.AppendIf(true, string.Empty))
            .ThrowsExactly<ArgumentException>();
    }

    [Test]
    public async Task AppendIf_TypeConstructExampleShouldSucceed()
    {
        var sb = new StringBuilder();
        var type = typeof(StringBuilderExtensionsTests);

        sb
            .AppendIf(type.IsNotPublic, "internal")
            .AppendIf(type.IsPublic, "public")
            .Append(' ')
            .AppendIf(type.IsClass, "class")
            .AppendIf(type.IsByRef, "struct")
            .Append(' ')
            .Append(type.Name);

        var result = sb.ToString();
        await Assert.That(result)
            .IsEqualTo($"public class {nameof(StringBuilderExtensionsTests)}");
    }


    [Test]
    public async Task EndsWith_ShouldReturnTrue_WhenSuffixIsPresent()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "World!";
        var result = sb.EndsWith(suffix);
        await Assert.That(result)
            .IsTrue();

        sb.Clear().Append("Hello\r\n");
        suffix = "\r\n";
        result = sb.EndsWith(suffix);
        await Assert.That(result)
            .IsTrue();
    }

    [Test]
    public async Task EndsWith_ShouldReturnFalse_WhenSuffixIsNotPresent()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "Universe!";
        var result = sb.EndsWith(suffix);
        await Assert.That(result)
            .IsFalse();
    }

    [Test]
    public async Task EndsWith_ShouldReturnTrue_WhenSuffixIsPresentWithCaseInsensitiveComparison()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "world!";
        var result = sb.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase);
        await Assert.That(result)
            .IsTrue();
    }

    [Test]
    public async Task EndsWith_ShouldReturnFalse_WhenSuffixIsNotPresentWithCaseInsensitiveComparison()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "universe!";
        var result = sb.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase);
        await Assert.That(result)
            .IsFalse();
    }

    [Test]
    public async Task EndsWith_ShouldThrow_WhenNullStringBuilderIsGiven()
    {
        StringBuilder sb = null!;
        await Assert.That(() => sb.EndsWith("World!"))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task EndsWith_ShouldThrow_WhenInvalidSuffixValueIsGiven()
    {
        var sb = new StringBuilder("Hello, World!");
        await Assert.That(() => sb.EndsWith(string.Empty))
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => sb.EndsWith(null!))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task EndsWith_ShouldReturnFalse_WhenSuffixIsLongerThanStringBuilder()
    {
        var sb = new StringBuilder("Hello, World!");
        var suffix = "ThisIsALongSuffix";
        var result = sb.EndsWith(suffix);
        await Assert.That(result)
            .IsFalse();
    }

    [Test]
    public async Task AppendLineIf_ShouldAppendValueWithNewLine_WhenConditionIsTrue()
    {
        var sb = new StringBuilder("Hello ");
        var condition = true;
        var valueToAppend = "World";

        sb.AppendLineIf(condition, valueToAppend);

        await Assert.That(sb.ToString())
            .IsEqualTo("Hello " + "World" + Environment.NewLine);
    }

    [Test]
    public async Task AppendLineIf_ShouldNotAppendValue_WhenConditionIsFalse()
    {
        var sb = new StringBuilder("Hello");
        var condition = false;
        var valueToAppend = "World";

        sb.AppendLineIf(condition, valueToAppend);

        await Assert.That(sb.ToString())
            .IsEqualTo("Hello");
    }

    [Test]
    public async Task AppendLineIf_ShouldThrow_WhenNullStringBuilderIsGiven()
    {
        StringBuilder sb = null!;
        await Assert.That(() => sb.AppendLineIf(true, "World"))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task AppendLineIf_ShouldThrow_WhenNullValueIsGiven()
    {
        var sb = new StringBuilder("Hello");
        await Assert.That(() => sb.AppendLineIf(true, null!))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task AppendLineIf_ShouldAppendEmptyStringWithNewLine_WhenValueIsEmpty()
    {
        var sb = new StringBuilder("Hello");
        var condition = true;

        sb.AppendLineIf(condition, string.Empty);
        await Assert.That(sb.ToString())
            .IsEqualTo("Hello" + Environment.NewLine);
    }

    [Test]
    public async Task AppendLines_ShouldAppendMultipleLines_WhenGivenMultipleStrings()
    {
        var sb = new StringBuilder($"Initial line{Environment.NewLine}");
        var lines = new[] { "First line", "Second line", "Third line" };

        sb.AppendLines(lines);
        await Assert.That(sb.ToString())
            .IsEqualTo($"Initial line{Environment.NewLine}First line{Environment.NewLine}Second line{Environment.NewLine}Third line{Environment.NewLine}");
    }

    [Test]
    public async Task AppendLines_ShouldNotModifyStringBuilder_WhenGivenEmptyCollection()
    {
        var sb = new StringBuilder("Initial content");
        var lines = Array.Empty<string>();

        sb.AppendLines(lines);

        await Assert.That(sb.ToString())
            .IsEqualTo("Initial content");
    }

    [Test]
    public async Task AppendLines_ShouldThrowArgumentNullException_WhenStringBuilderIsNull()
    {
        StringBuilder sb = null!;
        var lines = new[] { "Some line" };

        await Assert.That(() => sb.AppendLines(lines))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("sb");
    }

    [Test]
    public async Task AppendLines_ShouldThrowArgumentNullException_WhenLinesCollectionIsNull()
    {
        var sb = new StringBuilder("Initial content");
        IEnumerable<string> lines = null!;

        await Assert.That(() => sb.AppendLines(lines))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("lines");
    }

    [Test]
    public async Task AppendLines_ShouldHandleNewLineForSingleElement_WhenGivenSingleString()
    {
        var sb = new StringBuilder($"Start{Environment.NewLine}");
        var lines = new[] { "Only line" };

        sb.AppendLines(lines);
        await Assert.That(sb.ToString())
            .IsEqualTo($"Start{Environment.NewLine}Only line{Environment.NewLine}");
    }
}
