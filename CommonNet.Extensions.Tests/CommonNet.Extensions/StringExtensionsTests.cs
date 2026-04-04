namespace CommonNet.Extensions.Tests;

public class StringExtensionsTests
{
    [Test]
    [StringGenerator(100, AllowEmpty: true)]
    public async Task String_IsNotNullOrXTest(string data)
    {
        await Assert.That(data.IsNotNullOrEmpty())
            .IsEqualTo(!string.IsNullOrEmpty(data));
        await Assert.That(data.IsNotNullOrWhiteSpace())
            .IsEqualTo(!string.IsNullOrWhiteSpace(data));
    }

    [Test]
    public async Task String_ParseException()
    {
        await Assert.That(() => "test".Parse<object>())
            .ThrowsExactly<NotSupportedException>();

        await Assert.That(() => "test".Parse<Enum>())
            .ThrowsExactly<NotSupportedException>();

        await Assert.That(() => "test".TryParse(out byte _))
            .IsFalse();
    }

    [Test]
    public async Task String_ParseByte()
    {
        await Assert.That(() => "aaa".Parse<byte>())
            .ThrowsExactly<FormatException>();

        await Assert.That(() => "123".Parse<byte>())
            .IsEqualTo((byte)123);

        await Assert.That(() => "1234".Parse<byte>())
            .ThrowsExactly<OverflowException>();

        await Assert.That(() => "\t\t 123    \v   ".Parse<byte>())
            .IsEqualTo((byte)123);

        await Assert.That(() => "123".TryParse(out byte _))
            .IsTrue();

        var result = "1234".TryParse(out byte val);
        await Assert.That(result)
            .IsFalse();
        await Assert.That(val)
            .IsEqualTo((byte)0);
    }

    enum TestEnum
    {
        Default,
        One
    }

    [Test]
    public async Task String_ParseEnum()
    {
        await Assert.That(() => "twelve".ParseToEnum<TestEnum>())
            .IsEqualTo(TestEnum.Default);
        await Assert.That(() => "one".ParseToEnum<TestEnum>())
            .IsEqualTo(TestEnum.One);
        await Assert.That(() => "one".ParseToEnum<TestEnum>(ignoreCase: false ))
            .IsEqualTo(TestEnum.Default);
        await Assert.That(() => "One".ParseToEnum<TestEnum>(ignoreCase: false))
            .IsEqualTo(TestEnum.One);
    }

    [Test]
    public async Task String_Repeat()
    {
        const string? nullStr = null;
        await Assert.That(() => nullStr!.Repeat(5))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => " ".Repeat(-5))
            .ThrowsExactly<ArgumentOutOfRangeException>();

        await Assert.That(() => " ".Repeat(0))
            .IsEqualTo(string.Empty);

        await Assert.That(() => "0123456789".Repeat(1))
            .IsEqualTo("0123456789");
        await Assert.That(() => "-".Repeat(10))
            .IsEqualTo("----------");
        await Assert.That(() => "00".Repeat(6, ":"))
            .IsEqualTo("00:00:00:00:00:00");
        await Assert.That(() => "+".Repeat(7, "-"))
            .IsEqualTo("+-+-+-+-+-+-+");
        await Assert.That(() => "X".Repeat(2).Repeat(2, ", ").Repeat(2, "-").Repeat(2, ":"))
            .IsEqualTo("XX, XX-XX, XX:XX, XX-XX, XX");
        await Assert.That(() => string.Empty.Repeat(10))
            .IsEqualTo(string.Empty);
    }

    [Test]
    public async Task String_TabsToSpaces()
    {
        const string? nullStr = null;
        await Assert.That(() => nullStr!.TabsToSpaces(2))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => " ".TabsToSpaces(-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert.That(() => "\t".TabsToSpaces(0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert.That(() => "text".TabsToSpaces(2))
            .IsEqualTo("text");
        await Assert.That(() => "\t\t".TabsToSpaces(2))
            .IsEqualTo("    ");
        await Assert.That(() => "\ttext".TabsToSpaces(4))
            .IsEqualTo("    text");
        await Assert.That(() => "\ttext".TabsToSpaces(1))
            .IsEqualTo(" text");
        await Assert.That(() => "\t\tstart of text\tend of text".TabsToSpaces(2))
            .IsEqualTo("    start of text  end of text");
    }

    [Test]
    public async Task String_GetBeforeOrEmpty()
    {
        const string? nullStr = null;
        await Assert.That(() => nullStr!.GetBeforeOrEmpty("."))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "test.me".GetBeforeOrEmpty(null!))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "test.me".GetBeforeOrEmpty(string.Empty))
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => "test.me".GetBeforeOrEmpty("."))
            .IsEqualTo("test");
        await Assert.That(() => "test.me".GetBeforeOrEmpty(".m"))
            .IsEqualTo("test");
        await Assert.That(() => "test.me".GetBeforeOrEmpty(","))
            .IsEqualTo(string.Empty);
        await Assert.That(() => "test.me".GetBeforeOrEmpty("t"))
            .IsEqualTo(string.Empty);
    }

    [Test]
    public async Task String_GetAfterOrEmpty()
    {
        const string? nullStr = null;
        await Assert.That(() => nullStr!.GetAfterOrEmpty("."))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "test.me".GetAfterOrEmpty(null!))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "test.me".GetAfterOrEmpty(string.Empty))
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => "test.me".GetAfterOrEmpty("."))
            .IsEqualTo("me");
        await Assert.That(() => "test.me".GetAfterOrEmpty(".m"))
            .IsEqualTo("e");
        await Assert.That(() => "test.me".GetAfterOrEmpty(","))
            .IsEqualTo(string.Empty);
        await Assert.That(() => "test.me".GetAfterOrEmpty("e"))
            .IsEqualTo(string.Empty);
    }

    [Test]
    public async Task String_GetBetweenOrEmpty()
    {
        const string? nullStr = null;
        await Assert.That(() => nullStr!.GetBetweenOrEmpty(".", "."))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(null!, "."))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(string.Empty, "."))
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(".", null!))
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(".", string.Empty))
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(".", "."))
            .IsEqualTo("test");
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(".", ","))
            .IsEqualTo(string.Empty);
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(",", "."))
            .IsEqualTo(string.Empty);
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(",", ","))
            .IsEqualTo(string.Empty);
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(".test", "."))
            .IsEqualTo(string.Empty);
        await Assert.That(() => "me.test.me".GetBetweenOrEmpty(".tes", "."))
            .IsEqualTo("t");
    }

    [Test]
    public async Task String_AllIndexesOf()
    {
        const string? nullStr = null;
        await Assert.That(() => nullStr!.AllIndexesOf(" ").ToArray())
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "test".AllIndexesOf(null!).ToArray())
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => "test".AllIndexesOf("").ToArray())
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => "".AllIndexesOf(" ").ToArray())
            .IsEmpty();
        await Assert.That(() => "test".AllIndexesOf("tset").ToArray())
            .IsEmpty();
        await Assert.That(() => "test".AllIndexesOf("t").ToArray())
            .IsEquivalentTo([0, 3]);
        await Assert.That(() => "test".AllIndexesOf("T", ignoreCase: true).ToArray())
            .IsEquivalentTo([0, 3]);
        await Assert.That(() => "test".AllIndexesOf("st", ignoreCase: false).ToArray())
            .IsEquivalentTo([2]);
        await Assert.That(() => "test".AllIndexesOf("St", ignoreCase: true).ToArray())
            .IsEquivalentTo([2]);
        await Assert.That(() => "test".AllIndexesOf("tt", ignoreCase: false).ToArray())
            .IsEmpty();
        await Assert.That(() => "test".AllIndexesOf("tT", ignoreCase: true).ToArray())
            .IsEmpty();
        await Assert.That(() => "test\r\nnew\r\nlines\r\n".AllIndexesOf("\r\n").ToArray())
            .IsEquivalentTo([4, 9, 16]);
    }

    [Test]
    public async Task Char_Repeat_ZeroTimes_ReturnsEmptyString()
    {
        await Assert.That(() => 'a'.Repeat(0))
            .IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Char_Repeat_NegativeTimes_ThrowsArgumentOutOfRangeException()
    {
        await Assert.That(() => 'a'.Repeat(-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Char_Repeat_PositiveTimes_ReturnsRepeatedCharacterString()
    {
        await Assert.That(() => 'a'.Repeat(5))
            .IsEqualTo("aaaaa");
    }

    [Test]
    public async Task Char_Repeat_OneTime_ReturnsSingleCharacterString()
    {
        await Assert.That(() => 'b'.Repeat(1))
            .IsEqualTo("b");
    }

    [Test]
    public async Task String_PadRight_WithDefaultSpaceCharacter()
    {
        await Assert.That(() => "test".PadRight(10))
            .IsEqualTo("test      ");
    }

    [Test]
    public async Task String_PadRight_WithCustomCharacter()
    {
        await Assert.That(() => "test".PadRight(10, '-'))
            .IsEqualTo("test------");
    }

    [Test]
    public async Task String_PadRight_WithNoPaddingNeeded()
    {
        await Assert.That(() => "test".PadRight(4))
            .IsEqualTo("test");
    }

    [Test]
    public async Task String_PadRight_WithLengthLessThanString()
    {
        await Assert.That(() => "test".PadRight(2))
            .IsEqualTo("test");
    }

    [Test]
    public async Task String_PadRight_WithNullString_ThrowsArgumentNullException()
    {
        await Assert.That(() => System.StringExtensions.PadRight(null!, 10))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task String_PadRight_WithNegativeTotalLength_ThrowsArgumentOutOfRangeException()
    {
        await Assert.That(() => "test".PadRight(-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task String_PadLeft_WithDefaultSpaceCharacter()
    {
        await Assert.That(() => "test".PadLeft(10))
            .IsEqualTo("      test");
    }

    [Test]
    public async Task String_PadLeft_WithCustomCharacter()
    {
        await Assert.That(() => "test".PadLeft(10, '-'))
            .IsEqualTo("------test");
    }

    [Test]
    public async Task String_PadLeft_WithNoPaddingNeeded()
    {
        await Assert.That(() => "test".PadLeft(4))
            .IsEqualTo("test");
    }

    [Test]
    public async Task String_PadLeft_WithLengthLessThanString()
    {
        await Assert.That(() => "test".PadLeft(2))
            .IsEqualTo("test");
    }

    [Test]
    public async Task String_PadLeft_WithNullString_ThrowsArgumentNullException()
    {
        await Assert.That(() => System.StringExtensions.PadLeft(null!, 10))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task String_PadLeft_WithNegativeTotalLength_ThrowsArgumentOutOfRangeException()
    {
        await Assert.That(() => "test".PadLeft(-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }
}
