namespace Tests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    [TestFixture]
    public class StringExtensions
    {
        [FsCheck.NUnit.Property(MaxTest = 100, Description = nameof(String_IsNotNullOrXTest), QuietOnSuccess = true)]
        public void String_IsNotNullOrXTest(string data)
        {
            Assert.AreEqual(data.IsNotNullOrEmpty(), !string.IsNullOrEmpty(data));
            Assert.AreEqual(data.IsNotNullOrWhiteSpace(), !string.IsNullOrWhiteSpace(data));
        }

        [Test]
        public void String_ParseException()
        {
            Assert.Catch<NotSupportedException>(() => "test".Parse<object>());
            Assert.Catch<NotSupportedException>(() => "test".Parse<Enum>());
            object o;
            Assert.AreEqual("test".TryParse<object>(out o), false);
        }

        [Test]
        public void String_ParseByte()
        {
            Assert.Catch<FormatException>(() => "aaa".Parse<byte>());
            Assert.AreEqual("123".Parse<byte>(), 123);
            Assert.Catch<OverflowException>(() => "1234".Parse<byte>());
            Assert.AreEqual("\t\t 123    \v   ".Parse<byte>(), 123);
            byte val;
            Assert.AreEqual("123".TryParse(out val), true);
            Assert.AreEqual(val, 123);
            Assert.AreEqual("1234".TryParse(out val), false);
            Assert.AreEqual(val, 0);
        }

        enum TestEnum
        {
            Default,
            One
        }

        [Test]
        public void String_ParseEnum()
        {
            Assert.AreEqual("twelve".ParseToEnum<TestEnum>(), TestEnum.Default);
            Assert.AreEqual("one".ParseToEnum<TestEnum>(), TestEnum.One);
            Assert.AreEqual("one".ParseToEnum<TestEnum>(false), TestEnum.Default);
            Assert.AreEqual("One".ParseToEnum<TestEnum>(false), TestEnum.One);

        }

        [Test]
        public void String_Repeat()
        {
            string nullStr = null;
            Assert.Catch<ArgumentException>(() => nullStr.Repeat(5));
            Assert.Catch<ArgumentException>(() => " ".Repeat(-5));
            Assert.AreEqual("0123456789".Repeat(0), string.Empty);
            Assert.AreEqual("-".Repeat(10), "----------");
            Assert.AreEqual("00".Repeat(6, ":"), "00:00:00:00:00:00");
            Assert.AreEqual("+".Repeat(7, "-"), "+-+-+-+-+-+-+");
            Assert.AreEqual("X".Repeat(2).Repeat(2, ", ").Repeat(2, "-").Repeat(2, ":"), "XX, XX-XX, XX:XX, XX-XX, XX");
            Assert.AreEqual(string.Empty.Repeat(10), string.Empty);
        }

        [Test]
        public void String_TabsToSpaces()
        {
            string nullStr = null;
            Assert.Catch<ArgumentException>(() => nullStr.TabsToSpaces(2));
            Assert.Catch<ArgumentException>(() => "\t".TabsToSpaces(-1));
            Assert.AreEqual("text".TabsToSpaces(2), "text");
            Assert.AreEqual("\t\t".TabsToSpaces(2), "    ");
            Assert.AreEqual("\ttext".TabsToSpaces(4), "    text");
            Assert.AreEqual("\ttext".TabsToSpaces(0), "text");
            Assert.AreEqual("\t\tstart of text\tend of text".TabsToSpaces(2), "    start of text  end of text");
        }

        [Test]
        public void String_GetBeforeOrEmpty()
        {
            string nullStr = null;
            Assert.Catch<ArgumentException>(() => nullStr.GetBeforeOrEmpty("."));
            Assert.Catch<InvalidDataException>(() => "test.me".GetBeforeOrEmpty(null));
            Assert.Catch<InvalidDataException>(() => "test.me".GetBeforeOrEmpty(string.Empty));
            Assert.AreEqual("test.me".GetBeforeOrEmpty("."), "test");
            Assert.AreEqual("test.me".GetBeforeOrEmpty(".m"), "test");
            Assert.AreEqual("test.me".GetBeforeOrEmpty(","), string.Empty);
            Assert.AreEqual("test.me".GetBeforeOrEmpty("t"), string.Empty);
        }

        [Test]
        public void String_GetAfterOrEmpty()
        {
            string nullStr = null;
            Assert.Catch<ArgumentException>(() => nullStr.GetAfterOrEmpty("."));
            Assert.Catch<InvalidDataException>(() => "test.me".GetAfterOrEmpty(null));
            Assert.Catch<InvalidDataException>(() => "test.me".GetAfterOrEmpty(string.Empty));
            Assert.AreEqual("test.me".GetAfterOrEmpty("."), "me");
            Assert.AreEqual("test.me".GetAfterOrEmpty(".m"), "e");
            Assert.AreEqual("test.me".GetAfterOrEmpty(","), string.Empty);
            Assert.AreEqual("test.me".GetAfterOrEmpty("e"), string.Empty);
        }

        [Test]
        public void String_GetBetweenOrEmpty()
        {
            string nullStr = null;
            Assert.Catch<ArgumentException>(() => nullStr.GetBetweenOrEmpty(".", "."));
            Assert.Catch<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(null, "."));
            Assert.Catch<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(string.Empty, "."));
            Assert.Catch<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(".", null));
            Assert.Catch<InvalidDataException>(() => "me.test.me".GetBetweenOrEmpty(".", string.Empty));
            Assert.AreEqual("me.test.me".GetBetweenOrEmpty(".", "."), "test");
            Assert.AreEqual("me.test.me".GetBetweenOrEmpty(".", ","), string.Empty);
            Assert.AreEqual("me.test.me".GetBetweenOrEmpty(",", "."), string.Empty);
            Assert.AreEqual("me.test.me".GetBetweenOrEmpty(",", ","), string.Empty);
            Assert.AreEqual("me.test.me".GetBetweenOrEmpty(".test", "."), string.Empty);
            Assert.AreEqual("me.test.me".GetBetweenOrEmpty(".tes", "."), "t");
        }
    }
}
