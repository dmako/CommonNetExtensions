namespace CommonNet.Extensions.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Xunit;

    public class StringExtensions
    {
        [FsCheck.Xunit.Property(MaxTest = 100, DisplayName = nameof(String_IsNotNullOrXTest), QuietOnSuccess = true)]
        public void String_IsNotNullOrXTest(string data)
        {
            Assert.Equal(data.IsNotNullOrEmpty(), !string.IsNullOrEmpty(data));
            Assert.Equal(data.IsNotNullOrWhiteSpace(), !string.IsNullOrWhiteSpace(data));
        }

        [Fact]
        public void String_ParseException()
        {
            Assert.Throws<NotSupportedException>(() => "test".Parse<object>());
            Assert.Throws<NotSupportedException>(() => "test".Parse<Enum>());
            Assert.False("test".TryParse(out object o));
        }

        [Fact]
        public void String_ParseByte()
        {
            Assert.Throws<FormatException>(() => "aaa".Parse<byte>());
            Assert.Equal(123, "123".Parse<byte>());
            Assert.Throws<OverflowException>(() => "1234".Parse<byte>());
            Assert.Equal(123, "\t\t 123    \v   ".Parse<byte>());
            Assert.True("123".TryParse(out byte val));
            Assert.Equal(123, val);
            Assert.False("1234".TryParse(out val));
            Assert.Equal(0, val);
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
            const string nullStr = null;
            Assert.Throws<ArgumentNullException>(() => nullStr.Repeat(5));
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
            const string nullStr = null;
            Assert.Throws<ArgumentNullException>(() => nullStr.TabsToSpaces(2));
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
            const string nullStr = null;
            Assert.Throws<ArgumentNullException>(() => nullStr.GetBeforeOrEmpty("."));
            Assert.Throws<InvalidDataException>(() => "test.me".GetBeforeOrEmpty(null));
            Assert.Throws<InvalidDataException>(() => "test.me".GetBeforeOrEmpty(string.Empty));
            Assert.Equal("test", "test.me".GetBeforeOrEmpty("."));
            Assert.Equal("test", "test.me".GetBeforeOrEmpty(".m"));
            Assert.Equal(string.Empty, "test.me".GetBeforeOrEmpty(","));
            Assert.Equal(string.Empty, "test.me".GetBeforeOrEmpty("t"));
        }

        [Fact]
        public void String_GetAfterOrEmpty()
        {
            const string nullStr = null;
            Assert.Throws<ArgumentNullException>(() => nullStr.GetAfterOrEmpty("."));
            Assert.Throws<InvalidDataException>(() => "test.me".GetAfterOrEmpty(null));
            Assert.Throws<InvalidDataException>(() => "test.me".GetAfterOrEmpty(string.Empty));
            Assert.Equal("me", "test.me".GetAfterOrEmpty("."));
            Assert.Equal("e", "test.me".GetAfterOrEmpty(".m"));
            Assert.Equal(string.Empty, "test.me".GetAfterOrEmpty(","));
            Assert.Equal(string.Empty, "test.me".GetAfterOrEmpty("e"));
        }

        [Fact]
        public void String_GetBetweenOrEmpty()
        {
            const string nullStr = null;
            Assert.Throws<ArgumentNullException>(() => nullStr.GetBetweenOrEmpty(".", "."));
            Assert.Throws<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(null, "."));
            Assert.Throws<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(string.Empty, "."));
            Assert.Throws<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(".", null));
            Assert.Throws<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(".", string.Empty));
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
            const string nullStr = null;
            Assert.Throws<ArgumentNullException>(() => nullStr.AllIndexesOf(" ").ToArray());
            Assert.Throws<ArgumentNullException>(() => "test".AllIndexesOf(null).ToArray());
            Assert.Throws<InvalidDataException>(() => "test".AllIndexesOf("").ToArray());
            Assert.Equal("".AllIndexesOf(" ").ToArray(), new int[] { });
            Assert.Equal("test".AllIndexesOf("tset").ToArray(), new int[] { });
            Assert.Equal("test".AllIndexesOf("t").ToArray(), new int[] { 0, 3 });
            Assert.Equal("test".AllIndexesOf("T", true).ToArray(), new int[] { 0, 3 });
            Assert.Equal("test".AllIndexesOf("st").ToArray(), new int[] { 2 });
            Assert.Equal("test".AllIndexesOf("St", true).ToArray(), new int[] { 2 });
            Assert.Equal("tttt".AllIndexesOf("tt").ToArray(), new int[] { 0, 1, 2 });
            Assert.Equal("tttt".AllIndexesOf("tT", true).ToArray(), new int[] { 0, 1, 2 });
            Assert.Equal("test\r\nnew\r\nlines\r\n".AllIndexesOf("\r\n").ToArray(), new int[] { 4, 9, 16 });
        }
    }
}
