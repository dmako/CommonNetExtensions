using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class StringExtensions
{
    [Property(MaxTest = 100, DisplayName = nameof(String_IsNotNullOrXTest), QuietOnSuccess = true)]
    public void String_IsNotNullOrXTest(string data)
    {
        Assert.Equal(data.IsNotNullOrEmpty(), !string.IsNullOrEmpty(data));
        Assert.Equal(data.IsNotNullOrWhiteSpace(), !string.IsNullOrWhiteSpace(data));
    }

    [Fact]
    public void String_ParseException()
    {
        var parseJob = "test".Parse<object>;
        parseJob.Should().ThrowExactly<NotSupportedException>();
        parseJob = "test".Parse<Enum>;
        parseJob.Should().ThrowExactly<NotSupportedException>();

        "test".TryParse(out byte _).Should().BeFalse();
    }

    [Fact]
    public void String_ParseByte()
    {
        Assert.Throws<FormatException>(() => "aaa".Parse<byte>());
        "123".Parse<byte>().Should().Be(123);
        Assert.Throws<OverflowException>(() => "1234".Parse<byte>());
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
        Assert.Equal(TestEnum.Default, "twelve".ParseToEnum<TestEnum>());
        Assert.Equal(TestEnum.One, "one".ParseToEnum<TestEnum>());
        Assert.Equal(TestEnum.Default, "one".ParseToEnum<TestEnum>(false));
        Assert.Equal(TestEnum.One, "One".ParseToEnum<TestEnum>(false));
    }

    [Fact]
    public void String_Repeat()
    {
        const string? nullStr = null;
        Assert.Throws<ArgumentNullException>(() => nullStr!.Repeat(5));
        Assert.Throws<ArgumentException>(() => " ".Repeat(-5));
        Assert.Equal(string.Empty, "0123456789".Repeat(0));
        Assert.Equal("----------", "-".Repeat(10));
        Assert.Equal("00:00:00:00:00:00", "00".Repeat(6, ":"));
        Assert.Equal("+-+-+-+-+-+-+", "+".Repeat(7, "-"));
        Assert.Equal("XX, XX-XX, XX:XX, XX-XX, XX", "X".Repeat(2).Repeat(2, ", ").Repeat(2, "-").Repeat(2, ":"));
        Assert.Equal(string.Empty, string.Empty.Repeat(10));
    }

    [Fact]
    public void String_TabsToSpaces()
    {
        const string? nullStr = null;
        Assert.Throws<ArgumentNullException>(() => nullStr!.TabsToSpaces(2));
        Assert.Throws<ArgumentException>(() => "\t".TabsToSpaces(-1));
        Assert.Equal("text", "text".TabsToSpaces(2));
        Assert.Equal("    ", "\t\t".TabsToSpaces(2));
        Assert.Equal("    text", "\ttext".TabsToSpaces(4));
        Assert.Equal("text", "\ttext".TabsToSpaces(0));
        Assert.Equal("    start of text  end of text", "\t\tstart of text\tend of text".TabsToSpaces(2));
    }

    [Fact]
    public void String_GetBeforeOrEmpty()
    {
        const string? nullStr = null;
        Assert.Throws<ArgumentNullException>(() => nullStr!.GetBeforeOrEmpty("."));
        Assert.Throws<ArgumentException>(() => "test.me".GetBeforeOrEmpty(null!));
        Assert.Throws<ArgumentException>(() => "test.me".GetBeforeOrEmpty(string.Empty));
        Assert.Equal("test", "test.me".GetBeforeOrEmpty("."));
        Assert.Equal("test", "test.me".GetBeforeOrEmpty(".m"));
        Assert.Equal(string.Empty, "test.me".GetBeforeOrEmpty(","));
        Assert.Equal(string.Empty, "test.me".GetBeforeOrEmpty("t"));
    }

    [Fact]
    public void String_GetAfterOrEmpty()
    {
        const string? nullStr = null;
        Assert.Throws<ArgumentNullException>(() => nullStr!.GetAfterOrEmpty("."));
        Assert.Throws<ArgumentException>(() => "test.me".GetAfterOrEmpty(null!));
        Assert.Throws<ArgumentException>(() => "test.me".GetAfterOrEmpty(string.Empty));
        Assert.Equal("me", "test.me".GetAfterOrEmpty("."));
        Assert.Equal("e", "test.me".GetAfterOrEmpty(".m"));
        Assert.Equal(string.Empty, "test.me".GetAfterOrEmpty(","));
        Assert.Equal(string.Empty, "test.me".GetAfterOrEmpty("e"));
    }

    [Fact]
    public void String_GetBetweenOrEmpty()
    {
        const string? nullStr = null;
        Assert.Throws<ArgumentNullException>(() => nullStr!.GetBetweenOrEmpty(".", "."));
        Assert.Throws<ArgumentException>(() => "me.test.me".GetBetweenOrEmpty(null!, "."));
        Assert.Throws<ArgumentException>(() => "me.test.me".GetBetweenOrEmpty(string.Empty, "."));
        Assert.Throws<ArgumentException>(() => "me.test.me".GetBetweenOrEmpty(".", null!));
        Assert.Throws<ArgumentException>(() => "me.test.me".GetBetweenOrEmpty(".", string.Empty));
        Assert.Equal("test", "me.test.me".GetBetweenOrEmpty(".", "."));
        Assert.Equal(string.Empty, "me.test.me".GetBetweenOrEmpty(".", ","));
        Assert.Equal(string.Empty, "me.test.me".GetBetweenOrEmpty(",", "."));
        Assert.Equal(string.Empty, "me.test.me".GetBetweenOrEmpty(",", ","));
        Assert.Equal(string.Empty, "me.test.me".GetBetweenOrEmpty(".test", "."));
        Assert.Equal("t", "me.test.me".GetBetweenOrEmpty(".tes", "."));
    }

    [Fact]
    public void String_AllIndexesOf()
    {
        const string? nullStr = null;
        Assert.Throws<ArgumentNullException>(() => nullStr!.AllIndexesOf(" ").ToArray());
        Assert.Throws<ArgumentNullException>(() => "test".AllIndexesOf(null!).ToArray());
        Assert.Throws<ArgumentException>(() => "test".AllIndexesOf("").ToArray());
        Assert.Equal("".AllIndexesOf(" ").ToArray(), Array.Empty<int>());
        Assert.Equal("test".AllIndexesOf("tset").ToArray(), Array.Empty<int>());
        Assert.Equal("test".AllIndexesOf("t").ToArray(), new int[] { 0, 3 });
        Assert.Equal("test".AllIndexesOf("T", true).ToArray(), new int[] { 0, 3 });
        Assert.Equal("test".AllIndexesOf("st").ToArray(), new int[] { 2 });
        Assert.Equal("test".AllIndexesOf("St", true).ToArray(), new int[] { 2 });
        Assert.Equal("tttt".AllIndexesOf("tt").ToArray(), new int[] { 0, 1, 2 });
        Assert.Equal("tttt".AllIndexesOf("tT", true).ToArray(), new int[] { 0, 1, 2 });
        Assert.Equal("test\r\nnew\r\nlines\r\n".AllIndexesOf("\r\n").ToArray(), new int[] { 4, 9, 16 });
    }
}
