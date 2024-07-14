using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class StringExtensionsTests
{
    [Property(MaxTest = 100, DisplayName = nameof(String_IsNotNullOrXTest), QuietOnSuccess = true)]
    public void String_IsNotNullOrXTest(string data)
    {
        data.IsNotNullOrEmpty().Should().Be(!string.IsNullOrEmpty(data));
        data.IsNotNullOrWhiteSpace().Should().Be(!string.IsNullOrWhiteSpace(data));
    }

    [Fact]
    public void String_ParseException()
    {
        Action action = () => "test".Parse<object>();
        action.Should().ThrowExactly<NotSupportedException>();
        action = () => "test".Parse<Enum>();
        action.Should().ThrowExactly<NotSupportedException>();

        "test".TryParse(out byte _).Should().BeFalse();
    }

    [Fact]
    public void String_ParseByte()
    {
        Action action = () => "aaa".Parse<byte>();
        action.Should().ThrowExactly<FormatException>();
        "123".Parse<byte>().Should().Be(123);
        action = () => "1234".Parse<byte>();
        action.Should().ThrowExactly<OverflowException>();
        "\t\t 123    \v   ".Parse<byte>().Should().Be(123);
        "123".TryParse(out byte val).Should().BeTrue();
        val.Should().Be(123);
        "1234".TryParse(out val).Should().BeFalse();
        val.Should().Be(0);
    }

    enum TestEnum
    {
        Default,
        One
    }

    [Fact]
    public void String_ParseEnum()
    {
        TestEnum.Default.Should().Be("twelve".ParseToEnum<TestEnum>());
        TestEnum.One.Should().Be("one".ParseToEnum<TestEnum>());
        TestEnum.Default.Should().Be("one".ParseToEnum<TestEnum>(false));
        TestEnum.One.Should().Be("One".ParseToEnum<TestEnum>(false));
    }

    [Fact]
    public void String_Repeat()
    {
        const string? nullStr = null;
        Action action = () => nullStr!.Repeat(5);
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => " ".Repeat(-5);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();

        " ".Repeat(0).Should().Be(string.Empty);
        "0123456789".Should().Be("0123456789".Repeat(1));
        "----------".Should().Be("-".Repeat(10));
        "00:00:00:00:00:00".Should().Be("00".Repeat(6, ":"));
        "+-+-+-+-+-+-+".Should().Be("+".Repeat(7, "-"));
        "XX, XX-XX, XX:XX, XX-XX, XX".Should().Be("X".Repeat(2).Repeat(2, ", ").Repeat(2, "-").Repeat(2, ":"));
        string.Empty.Should().Be(string.Empty.Repeat(10));
    }

    [Fact]
    public void String_TabsToSpaces()
    {
        const string? nullStr = null;
        Action action = () => nullStr!.TabsToSpaces(2);
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "\t".TabsToSpaces(-1);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        action = () => "\t".TabsToSpaces(0);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        "text".Should().Be("text".TabsToSpaces(2));
        "    ".Should().Be("\t\t".TabsToSpaces(2));
        "    text".Should().Be("\ttext".TabsToSpaces(4));
        " text".Should().Be("\ttext".TabsToSpaces(1));
        "    start of text  end of text".Should().Be("\t\tstart of text\tend of text".TabsToSpaces(2));
    }

    [Fact]
    public void String_GetBeforeOrEmpty()
    {
        const string? nullStr = null;
        Action action = () => nullStr!.GetBeforeOrEmpty(".");
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "test.me".GetBeforeOrEmpty(null!);
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "test.me".GetBeforeOrEmpty(string.Empty);
        action.Should().ThrowExactly<ArgumentException>();
        "test".Should().Be("test.me".GetBeforeOrEmpty("."));
        "test".Should().Be("test.me".GetBeforeOrEmpty(".m"));
        string.Empty.Should().Be("test.me".GetBeforeOrEmpty(","));
        string.Empty.Should().Be("test.me".GetBeforeOrEmpty("t"));
    }

    [Fact]
    public void String_GetAfterOrEmpty()
    {
        const string? nullStr = null;
        Action action = () => nullStr!.GetAfterOrEmpty(".");
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "test.me".GetAfterOrEmpty(null!);
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "test.me".GetAfterOrEmpty(string.Empty);
        action.Should().ThrowExactly<ArgumentException>();
        "me".Should().Be("test.me".GetAfterOrEmpty("."));
        "e".Should().Be("test.me".GetAfterOrEmpty(".m"));
        string.Empty.Should().Be("test.me".GetAfterOrEmpty(","));
        string.Empty.Should().Be("test.me".GetAfterOrEmpty("e"));
    }

    [Fact]
    public void String_GetBetweenOrEmpty()
    {
        const string? nullStr = null;
        Action action = () => nullStr!.GetBetweenOrEmpty(".", ".");
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "me.test.me".GetBetweenOrEmpty(null!, ".");
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "me.test.me".GetBetweenOrEmpty(string.Empty, ".");
        action.Should().ThrowExactly<ArgumentException>();
        action = () => "me.test.me".GetBetweenOrEmpty(".", null!);
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => "me.test.me".GetBetweenOrEmpty(".", string.Empty);
        action.Should().ThrowExactly<ArgumentException>();
        "test".Should().Be("me.test.me".GetBetweenOrEmpty(".", "."));
        string.Empty.Should().Be("me.test.me".GetBetweenOrEmpty(".", ","));
        string.Empty.Should().Be("me.test.me".GetBetweenOrEmpty(",", "."));
        string.Empty.Should().Be("me.test.me".GetBetweenOrEmpty(",", ","));
        string.Empty.Should().Be("me.test.me".GetBetweenOrEmpty(".test", "."));
        "t".Should().Be("me.test.me".GetBetweenOrEmpty(".tes", "."));
    }

    [Fact]
    public void String_AllIndexesOf()
    {
        const string? nullStr = null;
        Action action = () => _ = nullStr!.AllIndexesOf(" ").ToArray();
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => _ = "test".AllIndexesOf(null!).ToArray();
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => _ = "test".AllIndexesOf("").ToArray();
        action.Should().ThrowExactly<ArgumentException>();
        "".AllIndexesOf(" ").ToArray().Should().BeEquivalentTo(Array.Empty<int>());
        "test".AllIndexesOf("tset").ToArray().Should().BeEquivalentTo(Array.Empty<int>());
        "test".AllIndexesOf("t").ToArray().Should().BeEquivalentTo(new int[] { 0, 3 });
        "test".AllIndexesOf("T", true).ToArray().Should().BeEquivalentTo(new int[] { 0, 3 });
        "test".AllIndexesOf("st").ToArray().Should().BeEquivalentTo(new int[] { 2 });
        "test".AllIndexesOf("St", true).ToArray().Should().BeEquivalentTo(new int[] { 2 });
        "tttt".AllIndexesOf("tt").ToArray().Should().BeEquivalentTo(new int[] { 0, 1, 2 });
        "tttt".AllIndexesOf("tT", true).ToArray().Should().BeEquivalentTo(new int[] { 0, 1, 2 });
        "test\r\nnew\r\nlines\r\n".AllIndexesOf("\r\n").ToArray().Should().BeEquivalentTo(new int[] { 4, 9, 16 });
    }

    [Fact]
    public void Char_Repeat_ZeroTimes_ReturnsEmptyString()
    {
        var result = 'a'.Repeat(0);
        result.Should().BeEmpty();
    }

    [Fact]
    public void Char_Repeat_NegativeTimes_ThrowsArgumentOutOfRangeException()
    {
        Action action = () => _ = 'a'.Repeat(-1);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Char_Repeat_PositiveTimes_ReturnsRepeatedCharacterString()
    {
        var result = 'a'.Repeat(5);
        result.Should().Be("aaaaa");
    }

    [Fact]
    public void Char_Repeat_OneTime_ReturnsSingleCharacterString()
    {
        var result = 'b'.Repeat(1);
        result.Should().Be("b");
    }

    [Fact]
    public void String_PadRight_WithDefaultSpaceCharacter()
    {
        var result = "test".PadRight(10);
        result.Should().Be("test      ");
    }

    [Fact]
    public void String_PadRight_WithCustomCharacter()
    {
        var result = "test".PadRight(10, '-');
        result.Should().Be("test------");
    }

    [Fact]
    public void String_PadRight_WithNoPaddingNeeded()
    {
        var result = "test".PadRight(4);
        result.Should().Be("test");
    }

    [Fact]
    public void String_PadRight_WithLengthLessThanString()
    {
        var result = "test".PadRight(2);
        result.Should().Be("test");
    }

    [Fact]
    public void String_PadRight_WithNullString_ThrowsArgumentNullException()
    {
        Action action = () => StringExtensions.PadRight(null!, 10);
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void String_PadRight_WithNegativeTotalLength_ThrowsArgumentOutOfRangeException()
    {
        Action action = () => "test".PadRight(-1);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void String_PadLeft_WithDefaultSpaceCharacter()
    {
        var result = "test".PadLeft(10);
        result.Should().Be("      test");
    }

    [Fact]
    public void String_PadLeft_WithCustomCharacter()
    {
        var result = "test".PadLeft(10, '-');
        result.Should().Be("------test");
    }

    [Fact]
    public void String_PadLeft_WithNoPaddingNeeded()
    {
        var result = "test".PadLeft(4);
        result.Should().Be("test");
    }

    [Fact]
    public void String_PadLeft_WithLengthLessThanString()
    {
        var result = "test".PadLeft(2);
        result.Should().Be("test");
    }

    [Fact]
    public void String_PadLeft_WithNullString_ThrowsArgumentNullException()
    {
        Action action = () => StringExtensions.PadLeft(null!, 10);
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void String_PadLeft_WithNegativeTotalLength_ThrowsArgumentOutOfRangeException()
    {
        Action action = () => "test".PadLeft(-1);
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

}
