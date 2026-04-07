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
    public async Task String_Parse_NullInput_ThrowsArgumentNullException()
    {
        string nullStr = null!;

        await Assert.That(() => nullStr.Parse<int>())
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task String_Parse_EmptyString_ReturnsDefault()
    {
        await Assert.That(() => "".Parse<int>())
            .IsEqualTo(0);
        await Assert.That(() => "".Parse<bool>())
            .IsFalse();
        await Assert.That(() => "".Parse<double>())
            .IsEqualTo(0.0);
        await Assert.That(() => "".Parse<Guid>())
            .IsEqualTo(Guid.Empty);
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

    [Test]
    public async Task String_ParseBool()
    {
        await Assert.That(() => "true".Parse<bool>())
            .IsTrue();
        await Assert.That(() => "True".Parse<bool>())
            .IsTrue();
        await Assert.That(() => "false".Parse<bool>())
            .IsFalse();
        await Assert.That(() => "notbool".Parse<bool>())
            .ThrowsExactly<FormatException>();
    }

    [Test]
    public async Task String_ParseSByte()
    {
        await Assert.That(() => "-128".Parse<sbyte>())
            .IsEqualTo(sbyte.MinValue);
        await Assert.That(() => "127".Parse<sbyte>())
            .IsEqualTo(sbyte.MaxValue);
        await Assert.That(() => "999".Parse<sbyte>())
            .ThrowsExactly<OverflowException>();
    }

    [Test]
    public async Task String_ParseChar()
    {
        await Assert.That(() => "A".Parse<char>())
            .IsEqualTo('A');
        await Assert.That(() => "hello".Parse<char>())
            .IsEqualTo('h');
    }

    [Test]
    public async Task String_ParseNumericTypes()
    {
        // short
        await Assert.That(() => "-32768".Parse<short>())
            .IsEqualTo(short.MinValue);
        await Assert.That(() => "32767".Parse<short>())
            .IsEqualTo(short.MaxValue);

        // ushort
        await Assert.That(() => "0".Parse<ushort>())
            .IsEqualTo(ushort.MinValue);
        await Assert.That(() => "65535".Parse<ushort>())
            .IsEqualTo(ushort.MaxValue);

        // int
        await Assert.That(() => "42".Parse<int>())
            .IsEqualTo(42);
        await Assert.That(() => "-100".Parse<int>())
            .IsEqualTo(-100);

        // uint
        await Assert.That(() => "42".Parse<uint>())
            .IsEqualTo(42u);

        // long
        await Assert.That(() => "9999999999".Parse<long>())
            .IsEqualTo(9999999999L);

        // ulong
        await Assert.That(() => "18446744073709551615".Parse<ulong>())
            .IsEqualTo(ulong.MaxValue);

        // double (invariant culture)
        await Assert.That(() => "3.14".Parse<double>())
            .IsEqualTo(3.14);
        await Assert.That(() => "-2.5".Parse<double>())
            .IsEqualTo(-2.5);
    }

    [Test]
    public async Task String_ParseDateTime()
    {
        await Assert.That(() => "2024-01-15".Parse<DateTime>())
            .IsEqualTo(new DateTime(2024, 1, 15));
        await Assert.That(() => "01/15/2024".Parse<DateTime>())
            .IsEqualTo(new DateTime(2024, 1, 15));
    }

    [Test]
    public async Task String_ParseTimeSpan()
    {
        await Assert.That(() => "01:30:00".Parse<TimeSpan>())
            .IsEqualTo(TimeSpan.FromMinutes(90));
        await Assert.That(() => "00:00:30".Parse<TimeSpan>())
            .IsEqualTo(TimeSpan.FromSeconds(30));
    }

    [Test]
    public async Task String_ParseGuid()
    {
        var guid = new Guid("12345678-1234-1234-1234-123456789abc");

        await Assert.That(() => "12345678-1234-1234-1234-123456789abc".Parse<Guid>())
            .IsEqualTo(guid);
    }

    [Test]
    public async Task String_ParseVersion()
    {
        await Assert.That(() => "1.2.3.4".Parse<Version>())
            .IsEqualTo(new Version(1, 2, 3, 4));
        await Assert.That(() => "2.0".Parse<Version>())
            .IsEqualTo(new Version(2, 0));
    }

    [Test]
    public async Task String_TryParse_NullInput_ThrowsArgumentNullException()
    {
        string nullStr = null!;

        await Assert.That(() => nullStr.TryParse(out int _))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    public async Task String_TryParse_UnsupportedType_ReturnsFalse()
    {
        var result = "test".TryParse(out object? val);
        await Assert.That(result)
            .IsFalse();
        await Assert.That(val)
            .IsNull();
    }

    [Test]
    public async Task String_TryParse_ValidValue_ReturnsTrueWithParsedValue()
    {
        var intResult = "42".TryParse(out int intVal);
        await Assert.That(intResult)
            .IsTrue();
        await Assert.That(intVal)
            .IsEqualTo(42);

        var boolResult = "true".TryParse(out bool boolVal);
        await Assert.That(boolResult)
            .IsTrue();
        await Assert.That(boolVal)
            .IsTrue();

        var doubleResult = "3.14".TryParse(out double doubleVal);
        await Assert.That(doubleResult)
            .IsTrue();
        await Assert.That(doubleVal)
            .IsEqualTo(3.14);
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
    public async Task String_AllIndexesOf_OverlappingMatches()
    {
        await Assert.That(() => "aaa".AllIndexesOf("aa").ToArray())
            .IsEquivalentTo([0, 1]);
        await Assert.That(() => "abab".AllIndexesOf("ab").ToArray())
            .IsEquivalentTo([0, 2]);
        await Assert.That(() => "xxxx".AllIndexesOf("xx").ToArray())
            .IsEquivalentTo([0, 1, 2]);
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
    public async Task String_PadRight_WithZeroTotalLength_ReturnsSameString()
    {
        await Assert.That(() => "test".PadRight(0))
            .IsEqualTo("test");
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

    [Test]
    public async Task String_PadLeft_WithZeroTotalLength_ReturnsSameString()
    {
        await Assert.That(() => "test".PadLeft(0))
            .IsEqualTo("test");
    }
}
