using System.Runtime.InteropServices;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class InteropExtensions
{
    struct T1
    {
        public byte V1;
        public byte V2;
        public byte V3;
        public byte V4;
    }

    [Fact]
    public void Marshal_BasicTests()
    {
        const byte[]? nullArray = null;
        Assert.Throws<ArgumentNullException>(() => nullArray!.BufferToStructure<T1>());
        Assert.Throws<ArgumentException>(() => new byte[] { 1, 2, 3 }.BufferToStructure<T1>());

        var val1 = new T1 { V1 = 1, V2 = 2, V3 = 3, V4 = 4 };
        var data = val1.StructureToBuffer();
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, data);
        var val2 = data.BufferToStructure<T1>();
        Assert.Equal(val1, val2);
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct T2
    {
        public T1 Nested;

        public fixed byte Data[4];
    }

    [Fact]
    public unsafe void Marshal_Tests()
    {
        Assert.Equal(8, Marshal.SizeOf(typeof(T2)));

        var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var t2 = data.BufferToStructure<T2>();

        Assert.Equal(1, t2.Nested.V1);
        Assert.Equal(2, t2.Nested.V2);
        Assert.Equal(3, t2.Nested.V3);
        Assert.Equal(4, t2.Nested.V4);
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, t2.Nested.StructureToBuffer());

        Assert.Equal(0, new ReadOnlySpan<byte>(t2.Data, 4).SequenceCompareTo(new byte[] { 5, 6, 7, 8 }));
    }
}
