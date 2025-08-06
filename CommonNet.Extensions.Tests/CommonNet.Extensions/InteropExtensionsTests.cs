using System.Runtime.InteropServices;
using TUnit.Assertions.AssertConditions.Throws;

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

    [Test]
    public async Task Marshal_BasicTests()
    {
        const byte[]? nullArray = null;
        await Assert.That(() => nullArray!.BufferToStructure<T1>())
            .ThrowsExactly<ArgumentNullException>();
        await Assert.That(() => new byte[] { 1, 2, 3 }.BufferToStructure<T1>())
            .ThrowsExactly<ArgumentOutOfRangeException>();

        var val1 = new T1 { V1 = 1, V2 = 2, V3 = 3, V4 = 4 };
        var data = val1.StructureToBuffer();
        await Assert.That(data)
            .IsEquivalentTo(new byte[] { 1, 2, 3, 4 });
        var val2 = data.BufferToStructure<T1>();
        await Assert.That(val1)
            .IsEquivalentTo(val2);
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct T2
    {
        public T1 Nested;

        public fixed byte Data[4];
    }

    [Test]
    public async Task Marshal_Tests()
    {
        await Assert.That(Marshal.SizeOf<T1>())
            .IsEqualTo(4);
        await Assert.That(Marshal.SizeOf<T2>())
            .IsEqualTo(8);

        var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var t2 = data.BufferToStructure<T2>();

        await Assert.That(t2.Nested.V1)
            .IsEqualTo((byte)1);
        await Assert.That(t2.Nested.V2)
            .IsEqualTo((byte)2);
        await Assert.That(t2.Nested.V3)
            .IsEqualTo((byte)3);
        await Assert.That(t2.Nested.V4)
            .IsEqualTo((byte)4);

        await Assert.That(t2.Nested.StructureToBuffer())
            .IsEquivalentTo(new byte[] { 1, 2, 3, 4 });
        await Assert.That(CapturePart(t2))
            .IsEquivalentTo(new byte[] { 5, 6, 7, 8 });
    }

    private unsafe byte[] CapturePart(T2 t2)
    {
        return new ReadOnlySpan<byte>(t2.Data, 4).ToArray();
    }
}
