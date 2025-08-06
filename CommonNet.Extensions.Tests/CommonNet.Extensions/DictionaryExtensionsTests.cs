using System.Threading.Tasks;

namespace CommonNet.Extensions.Tests;

public class DictionaryExtensionsTests
{
    readonly Func<int, int> factoryFnc = (key) => key;
    readonly Func<int, int> factoryFncPlusOne = (key) => key + 1;
    readonly Func<int, int, int> updateFnc = (key, old) => old + key;

    [Test]
    public async Task Dictionary_AddOrGetValueBasicTest()
    {
        var dict = new Dictionary<int, int>();
        var val = dict.GetOrAdd(1, 1);
        await Assert.That(val)
            .IsEqualTo(1);
        val = dict.GetOrAdd(1, 2);
        await Assert.That(val)
            .IsEqualTo(1);

        val = dict.GetOrAdd(2, factoryFnc);
        await Assert.That(val)
            .IsEqualTo(factoryFnc(2));
        val = dict.GetOrAdd(2, factoryFncPlusOne);
        await Assert.That(val)
            .IsEqualTo(factoryFnc(2));
    }

    [Test]
    public async Task Dictionary_AddOrGetLazyValueBasicTest()
    {
        var dict = new Dictionary<int, Lazy<int>>();
        var val = dict.GetOrAdd(3, factoryFnc);
        await Assert.That(val)
            .IsEqualTo(factoryFnc(3));
        val = dict.GetOrAdd(3, factoryFncPlusOne);
        await Assert.That(val)
            .IsEqualTo(factoryFnc(3));
    }

    [Test]
    public async Task Dictionary_AddOrUpdateValueBasicTest()
    {
        var dict = new Dictionary<int, int>();
        var val = dict.AddOrUpdate(1, 1, updateFnc);
        await Assert.That(val)
            .IsEqualTo(1);
        val = dict.AddOrUpdate(1, 1, updateFnc);
        await Assert.That(val)
            .IsEqualTo(updateFnc(1, 1));

        val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
        await Assert.That(val)
            .IsEqualTo(factoryFnc(2));;
        val = dict.AddOrUpdate(2, factoryFnc, updateFnc);
        await Assert.That(val)
            .IsEqualTo(updateFnc(2, 2));
    }

    [Test]
    public async Task Dictionary_AddOrUpdateLazyValueBasicTest()
    {
        var dict = new Dictionary<int, Lazy<int>>();
        var val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
        await Assert.That(val)
            .IsEqualTo(factoryFnc(3));
        val = dict.AddOrUpdate(3, factoryFnc, updateFnc);
        await Assert.That(val)
            .IsEqualTo(updateFnc(3, 3));
    }
}
