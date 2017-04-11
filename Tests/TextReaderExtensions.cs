namespace Tests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    [TestFixture]
    public class TextReaderExtensions
    {

        [Test]
        public void TextReader_BasicTests()
        {
            StreamReader nullReader = null;
            // Why the call is optimized out even in debug?
            //Assert.Catch<ArgumentNullException>(() => ((TextReader)null).EnumLines());
            Assert.Catch<ArgumentNullException>(() => nullReader.ForEachLine(l => { }));

            using (var reader = new StreamReader(new MemoryStream()))
            {
                Assert.Catch<ArgumentNullException>(() => reader.ForEachLine(null));
            }
        }

        [FsCheck.NUnit.Property(MaxTest = 100, Arbitrary = new[] { typeof(NonNullNoCrlLfStringArbitrary) }, Description = nameof(TextReader_PropertyEnumLines), QuietOnSuccess = true)]
        public void TextReader_PropertyEnumLines(string[] data)
        {
            var i = 0;
            var len = data.Length > 0 && data[data.Length - 1].Length == 0 ? data.Length - 1 : data.Length;
            using (var sr = new StringReader(String.Join("\n", data)))
            {
                foreach (var line in sr.EnumLines())
                {
                    Assert.AreEqual(line, data[i]);
                    i++;
                }
                Assert.AreEqual(i, len);
            }
        }


        [FsCheck.NUnit.Property(MaxTest = 100, Arbitrary = new[] { typeof(NonNullNoCrlLfStringArbitrary) }, Description = nameof(TextReader_ForEachLine), QuietOnSuccess = true)]
        public void TextReader_ForEachLine(string[] data)
        {
            var i = 0;
            var len = data.Length > 0 && data[data.Length - 1].Length == 0 ? data.Length - 1 : data.Length;
            using (var sr = new StringReader(String.Join("\n", data)))
            {
                sr.ForEachLine(line =>
                {
                    Assert.AreEqual(line, data[i]);
                    i++;
                });
                Assert.AreEqual(i, len);
            }
        }
    }
}

