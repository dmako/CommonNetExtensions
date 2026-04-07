namespace CommonNet.Extensions.Tests;

public class EqualityComparerFactoryTests
{
    private readonly struct TestStruct(int length)
    {
        public int Length { get; } = length;
    }

    private readonly struct TestStruct2(TestStruct @struct)
    {
        public TestStruct Struct { get; } = @struct;
    }

    [Test]
    public async Task CreateForRefType_TestCases_ShouldSucceed()
    {
        static bool stringEquals(string? x, string? y) => x?.Length == y?.Length;
        static int stringGetHashCode(string str) => str.Length.GetHashCode();

        var comparer = EqualityComparerFactory.Create<string>(stringEquals, stringGetHashCode);
        await Assert.That(comparer)
            .IsNotNull();
        await Assert.That(comparer.Equals("test", "test"))
            .IsTrue();
        await Assert.That(comparer.Equals("test", "ahoy"))
            .IsTrue();
        await Assert.That(comparer.GetHashCode("test"))
            .IsEqualTo(stringGetHashCode("test"));

        var comparer2 = EqualityComparerFactory.Create<string>(stringEquals);
        await Assert.That(comparer2)
            .IsNotNull();
        await Assert.That(() => comparer2.GetHashCode("test"))
            .ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task CreateForValueType_TestCases_ShouldSucceed()
    {
        static bool testStructEquals(TestStruct x, TestStruct y) => x.Length == y.Length;
        static int testStructGetHashCode(TestStruct ts) => ts.Length.GetHashCode();

        var comparer = EqualityComparerFactory.Create<TestStruct>(testStructEquals, testStructGetHashCode);
        await Assert.That(comparer)
            .IsNotNull();
        await Assert.That(comparer.Equals(new TestStruct(1), new TestStruct(1)))
            .IsTrue();
        await Assert.That(comparer.Equals(new TestStruct(1), new TestStruct(2)))
            .IsFalse();
        await Assert.That(comparer.GetHashCode(new TestStruct(1)))
            .IsEqualTo(testStructGetHashCode(new TestStruct(1)));

        var comparer2 = EqualityComparerFactory.Create<TestStruct>(testStructEquals);
        await Assert.That(comparer2)
            .IsNotNull();
        await Assert.That(() => comparer2.GetHashCode(new TestStruct(1)))
            .ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task CreateKeyedForRefType_TestCases_ShouldSucceed()
    {
        static bool customStringEquals(string? x, string? y) => x == y;
        static int customStringGetHashCode(string str) => str.GetHashCode();

        var comparerWithDefaults = EqualityComparerFactory.CreateKeyed<string, int>(s => s.Length);
        await Assert.That(comparerWithDefaults)
            .IsNotNull();
        await Assert.That(comparerWithDefaults.Equals("a", "b"))
            .IsTrue();
        await Assert.That(() => comparerWithDefaults.GetHashCode("test"))
            .ThrowsExactly<NotSupportedException>();

        var comparerWithDefaultsNoEquitableKey = EqualityComparerFactory.CreateKeyed<string, char[]>(s => s.ToCharArray());
        await Assert.That(comparerWithDefaultsNoEquitableKey)
            .IsNotNull();
        await Assert.That(() => comparerWithDefaultsNoEquitableKey.Equals("test", "test"))
            .ThrowsExactly<NotSupportedException>();

        var comparerWithCustomEquals = EqualityComparerFactory.CreateKeyed<string, string>(s => s, customStringEquals, customStringGetHashCode);
        await Assert.That(comparerWithCustomEquals)
            .IsNotNull();
        await Assert.That(comparerWithCustomEquals.Equals("test", "test"))
            .IsTrue();
        await Assert.That(comparerWithCustomEquals.GetHashCode("test"))
            .IsEqualTo(customStringGetHashCode("test"));
    }

    [Test]
    public async Task CreateKeyedForValueType_TestCases_ShouldSucceed()
    {
        static bool customTestStructureEquals(TestStruct x, TestStruct y) => x.Length == y.Length;
        static int customTestStructGetHashCode(TestStruct ts) => ts.Length.GetHashCode();

        var comparerWithDefaults = EqualityComparerFactory.CreateKeyed<TestStruct, int>(ts => ts.Length);
        await Assert.That(comparerWithDefaults)
            .IsNotNull();
        await Assert.That(comparerWithDefaults.Equals(new TestStruct(1), new TestStruct(1)))
            .IsTrue();
        await Assert.That(() => comparerWithDefaults.GetHashCode(new TestStruct(1)))
            .ThrowsExactly<NotSupportedException>();

        var comparerWithDefaultsNoEquitableKey = EqualityComparerFactory.CreateKeyed<TestStruct2, TestStruct>(ts => ts.Struct);
        await Assert.That(comparerWithDefaultsNoEquitableKey)
            .IsNotNull();
        await Assert.That(() => comparerWithDefaultsNoEquitableKey.Equals(new TestStruct2(new TestStruct(1)), new TestStruct2(new TestStruct(1))))
            .ThrowsExactly<NotSupportedException>();

        var comparerWithCustomEquals = EqualityComparerFactory.CreateKeyed<TestStruct2, TestStruct>(ts => ts.Struct, customTestStructureEquals, customTestStructGetHashCode);
        await Assert.That(comparerWithCustomEquals)
            .IsNotNull();
        await Assert.That(comparerWithCustomEquals.Equals(new TestStruct2(new TestStruct(1)), new TestStruct2(new TestStruct(1))))
            .IsTrue();
        await Assert.That(comparerWithCustomEquals.GetHashCode(new TestStruct2(new TestStruct(1))))
            .IsEqualTo(comparerWithCustomEquals.GetHashCode(new TestStruct2(new TestStruct(1))));
    }
}
