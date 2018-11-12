namespace Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Xunit;

    public class ByteArrayExtensions
    {
        [Fact]
        public void ByteArray_XorBasicTest()
        {
            const byte[] nullArr = null;
            var arr = new byte[] { 0x00 };
            var arr2 = new byte[] { 0x01, 0x02 };
            Assert.Throws<ArgumentNullException>(() => nullArr.Xor(0, 1, arr));
            Assert.Throws<ArgumentNullException>(() => arr.Xor(0, 1, null));
            Assert.Throws<InvalidDataException>(() => arr.Xor(0, 1, new byte[] { }));
            Assert.Throws<InvalidDataException>(() => arr.Xor(1, 1, arr));
            Assert.Throws<InvalidDataException>(() => arr2.Xor(1, 2, arr));
        }

        [FsCheck.Xunit.Property(MaxTest = 100, Arbitrary = new[] { typeof(NonEmptyArrayArbitrary) }, DisplayName = nameof(ByteArray_PropertyXorTest), QuietOnSuccess = true)]
        public void ByteArray_PropertyXorTest(byte[] data, byte[] key)
        {
            var local = new byte[data.Length];
            Array.Copy(data, local, data.Length);
            local.Xor(0, local.Length, key);
            Assert.False(data.SequenceEqual(local));
            local.Xor(0, local.Length, key);
            Assert.True(data.SequenceEqual(local));
        }
    }
}
