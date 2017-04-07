namespace Tests
{
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	[TestFixture]
	public class StringExtensions
	{
		[FsCheck.NUnit.Property(MaxTest = 100, Description = nameof(String_IsNotNullOrXTest), QuietOnSuccess = true)]
		public void String_IsNotNullOrXTest(string data)
		{
			Assert.AreEqual(data.IsNotNullOrEmpty(), !String.IsNullOrEmpty(data));
			Assert.AreEqual(data.IsNotNullOrWhiteSpace(), !String.IsNullOrWhiteSpace(data));
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
	}
}
