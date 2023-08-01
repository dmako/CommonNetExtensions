using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class InteropExtensionsTests
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
        Action action = () => nullArray!.BufferToStructure<T1>();
        action.Should().ThrowExactly<ArgumentNullException>();
        action = () => new byte[] { 1, 2, 3 }.BufferToStructure<T1>();
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();

        var val1 = new T1 { V1 = 1, V2 = 2, V3 = 3, V4 = 4 };
        var data = val1.StructureToBuffer();
        data.Should().BeEquivalentTo(new byte[] { 1, 2, 3, 4 });
        var val2 = data.BufferToStructure<T1>();
        val1.Should().BeEquivalentTo(val2);
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
        Marshal.SizeOf(typeof(T2)).Should().Be(8);

        var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var t2 = data.BufferToStructure<T2>();

        t2.Nested.V1.Should().Be(1);
        t2.Nested.V2.Should().Be(2);
        t2.Nested.V3.Should().Be(3);
        t2.Nested.V4.Should().Be(4);

        t2.Nested.StructureToBuffer().Should().BeEquivalentTo(new byte[] { 1, 2, 3, 4 });
        0.Should().Be(new ReadOnlySpan<byte>(t2.Data, 4).SequenceCompareTo(new byte[] { 5, 6, 7, 8 }));
    }
}
