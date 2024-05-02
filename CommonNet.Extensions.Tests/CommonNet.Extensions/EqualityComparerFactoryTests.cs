using FluentAssertions;
using Xunit;

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

    [Fact]
    public void CreateForRefType_TestCases_ShouldSucceed()
    {
        static bool stringEquals(string? x, string? y) => x?.Length == y?.Length;
        static int stringGetHashCode(string str) => str.Length.GetHashCode();

        var comparer = EqualityComparerFactory.Create<string>(stringEquals, stringGetHashCode);
        comparer.Should().NotBeNull();
        comparer.Equals("test", "test").Should().BeTrue();
        comparer.Equals("test", "ahoy").Should().BeTrue();
        comparer.GetHashCode("test").Should().Be(stringGetHashCode("test"));

        var comparer2 = EqualityComparerFactory.Create<string>(stringEquals);
        comparer2.Should().NotBeNull();
        var act = () => comparer2.GetHashCode("test");
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void CreateForValueType_TestCases_ShouldSucceed()
    {
        static bool testStructEquals(TestStruct x, TestStruct y) => x.Length == y.Length;
        static int testStructGetHashCode(TestStruct ts) => ts.Length.GetHashCode();

        var comparer = EqualityComparerFactory.Create<TestStruct>(testStructEquals, testStructGetHashCode);
        comparer.Should().NotBeNull();
        comparer.Equals(new TestStruct(1), new TestStruct(1)).Should().BeTrue();
        comparer.Equals(new TestStruct(1), new TestStruct(2)).Should().BeFalse();
        comparer.GetHashCode(new TestStruct(1)).Should().Be(testStructGetHashCode(new TestStruct(1)));

        var comparer2 = EqualityComparerFactory.Create<TestStruct>(testStructEquals);
        comparer2.Should().NotBeNull();
        var act = () => comparer2.GetHashCode(new TestStruct(1));
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void CreateKeyedForRefType_TestCases_ShouldSucceed()
    {
        static bool customStringEquals(string? x, string? y) => x == y;
        static int customStringGetHashCode(string str) => str.GetHashCode();

        var comparerWithDefaults = EqualityComparerFactory.CreateKeyed<string, int>(s => s.Length);
        comparerWithDefaults.Should().NotBeNull();
        comparerWithDefaults.Equals("a", "b").Should().BeTrue();
        var comparerWithDefaultsAct = () => comparerWithDefaults.GetHashCode("test");
        comparerWithDefaultsAct.Should().Throw<NotSupportedException>();

        var comparerWithDefaultsNoEquitableKey = EqualityComparerFactory.CreateKeyed<string, char[]>(s => s.ToCharArray());
        comparerWithDefaultsNoEquitableKey.Should().NotBeNull();
        var comparerWithDefaultsNoEquitableKeyAct = () => comparerWithDefaultsNoEquitableKey.Equals("test", "test");
        comparerWithDefaultsNoEquitableKeyAct.Should().Throw<NotSupportedException>();

        var comparerWithCustomEquals = EqualityComparerFactory.CreateKeyed<string, string>(s => s, customStringEquals, customStringGetHashCode);
        comparerWithCustomEquals.Should().NotBeNull();
        comparerWithCustomEquals.Equals("test", "test").Should().BeTrue();
        comparerWithCustomEquals.GetHashCode("test").Should().Be(comparerWithCustomEquals.GetHashCode("test"));
    }

    [Fact]
    public void CreateKeyedForValueType_TestCases_ShouldSucceed()
    {
        static bool customTestStructureEquals(TestStruct x, TestStruct y) => x.Length == y.Length;
        static int customTestStructGetHashCode(TestStruct ts) => ts.Length.GetHashCode();

        var comparerWithDefaults = EqualityComparerFactory.CreateKeyed<TestStruct, int>(ts => ts.Length);
        comparerWithDefaults.Should().NotBeNull();
        comparerWithDefaults.Equals(new TestStruct(1), new TestStruct(1)).Should().BeTrue();
        var comparerWithDefaultsAct = () => comparerWithDefaults.GetHashCode(new TestStruct(1));
        comparerWithDefaultsAct.Should().Throw<NotSupportedException>();

        var comparerWithDefaultsNoEquitableKey = EqualityComparerFactory.CreateKeyed<TestStruct2, TestStruct>(ts => ts.Struct);
        comparerWithDefaultsNoEquitableKey.Should().NotBeNull();
        var comparerWithDefaultsNoEquitableKeyAct = () => comparerWithDefaultsNoEquitableKey.Equals(new TestStruct2(new TestStruct(1)), new TestStruct2(new TestStruct(1)));
        comparerWithDefaultsNoEquitableKeyAct.Should().Throw<NotSupportedException>();

        var comparerWithCustomEquals = EqualityComparerFactory.CreateKeyed<TestStruct2, TestStruct>(ts => ts.Struct, customTestStructureEquals, customTestStructGetHashCode);
        comparerWithCustomEquals.Should().NotBeNull();
        comparerWithCustomEquals.Equals(new TestStruct2(new TestStruct(1)), new TestStruct2(new TestStruct(1))).Should().BeTrue();
        comparerWithCustomEquals.GetHashCode(new TestStruct2(new TestStruct(1))).Should().Be(comparerWithCustomEquals.GetHashCode(new TestStruct2(new TestStruct(1))));
    }
}
