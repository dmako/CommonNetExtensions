namespace Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;

    [TestFixture]
    public class ByteArrayExtensions
    {
        [Test]
        public void ByteArray_XorBasicTest()
        {
            const byte[] nullArr = null;
            var arr = new byte[] { 0x00 };
            var arr2 = new byte[] { 0x01, 0x02 };
            Assert.Catch<ArgumentNullException>(() => nullArr.Xor(0, 1, arr));
            Assert.Catch<ArgumentNullException>(() => arr.Xor(0, 1, null));
            Assert.Catch<InvalidDataException>(() => arr.Xor(0, 1, new byte[] { }));
            Assert.Catch<InvalidDataException>(() => arr.Xor(1, 1, arr));
            Assert.Catch<InvalidDataException>(() => arr2.Xor(1, 2, arr));
        }

        [FsCheck.NUnit.Property(MaxTest = 100, Arbitrary = new[] { typeof(NonEmptyArrayArbitrary) }, Description = nameof(ByteArray_PropertyXorTest), QuietOnSuccess = true)]
        public void ByteArray_PropertyXorTest(byte[] data, byte[] key)
        {
            var local = new byte[data.Length];
            Array.Copy(data, local, data.Length);
            local.Xor(0, local.Length, key);
            Assert.IsFalse(data.SequenceEqual(local));
            local.Xor(0, local.Length, key);
            Assert.IsTrue(data.SequenceEqual(local));
        }
    }
}
