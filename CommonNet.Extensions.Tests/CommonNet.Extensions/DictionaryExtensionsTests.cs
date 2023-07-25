using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;
public class DictionaryExtensionsTests
{
    readonly Func<int, int> factoryFnc = (key) => key;
    readonly Func<int, int> factoryFncPlusOne = (key) => key + 1;
    readonly Func<int, int, int> updateFnc = (key, old) => old + key;

    [Fact]
    public void Dictionary_AddOrGetValueBasicTest()
    {
        var dict = new Dictionary<int, int>();
        var val = dict.GetOrAdd(1, 1);
        val.Should().Be(1);
        val = dict.GetOrAdd(1, 2);
        val.Should().Be(1);

        val = dict.GetOrAdd(2, factoryFnc);
        val.Should().Be(factoryFnc(2));
        val = dict.GetOrAdd(2, factoryFncPlusOne);
        val.Should().Be(factoryFnc(2));
    }

    [Fact]
    public void Dictionary_AddOrGetLazyValueBasicTest()
    {
        var dict = new Dictionary<int, Lazy<int>>();
        var val = dict.GetOrAdd(3, factoryFnc);
        val.Should().Be(factoryFnc(3));
        val = dict.GetOrAdd(3, factoryFncPlusOne);
        val.Should().Be(factoryFnc(3));
    }

    [Fact]
    public void Dictionary_AddOrUpdateValueBasicTest()
    {
        var dict = new Dictionary<int, int>();
        var val = dict.AddOrUpdate(1, 1, updateFnc);
        val.Should().Be(1);
        val = dict.AddOrUpdate(1, 1, updateFnc);
        val.Should().Be(updateFnc(1, 1));

        val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
        val.Should().Be(factoryFnc(2));
        val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
        val.Should().Be(updateFnc(2, 2));
    }

    [Fact]
    public void Dictionary_AddOrUpdateLazyValueBasicTest()
    {
        var dict = new Dictionary<int, Lazy<int>>();
        var val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
        val.Should().Be(factoryFnc(3));
        val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
        val.Should().Be(updateFnc(3, 3));
    }
}
