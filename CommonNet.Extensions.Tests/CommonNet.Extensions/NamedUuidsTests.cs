namespace CommonNet.Extensions.Tests;

public class NamedUuidsTests
{
    public record NamedUuidTestInputPair(Guid NamespaceId, string Name);

    public static IEnumerable<NamedUuidTestInputPair> GetNamedUuidGenerationTestData()
    {
        yield return new(NamedUuid.IsoOidNamespace, "1.3.6.1.4.1.343");
        yield return new(NamedUuid.UrlNamespace, "https://wikipedia.org/");
        yield return new(NamedUuid.FqnNamespace, "my.computer.org");
        yield return new(NamedUuid.X500DnNamespace, "C=US;O=Example Organisation;CN=Test User 1");
        yield return new(NamedUuid.X500DnNamespace, "CN=My User;O=My Org;OU=Unit;C=AU;L=My Town;S=NSW;E=myuser@my.org");
        yield return new(Guid.Parse("00000000-dead-beef-f00d-000000000000"), "Private namespace and identificator");
    }


    [Test]
    [MethodDataSource(nameof(GetNamedUuidGenerationTestData))]
    public async Task Create_ShouldProduceSameOutput_WithSameInput(NamedUuidTestInputPair values)
    {
        var (namespaceId, name) = values;

        var uuid1 = NamedUuid.Create(namespaceId, name);
        var uuid2 = NamedUuid.Create(namespaceId, name);

        await Assert.That(uuid1)
            .IsEqualTo(uuid2);
    }

    [Test]
    [Arguments("6ba7b810-9dad-11d1-80b4-00c04fd430c8", "www.example.org", "74738ff5-5367-5958-9aee-98fffdcd1876")]
    [Arguments("b08bfdda-de67-4bad-b2d3-b6bb3a747761", "example name", "a04c2abb-0b48-5975-a981-c50d825a8610")]
    [Arguments("6ba7b810-9dad-11d1-80b4-00c04fd430c8", "my.computer.org", "3aa8cd3e-12cb-5d4f-9ad4-ee0774b4b93d")]
    public async Task Create_ShouldProduceSameOutput_BasedOnTestVectorsFromInternet(string namespaceId, string name, string expectedOutput)
    {
        var ns = Guid.Parse(namespaceId);
        var exp = Guid.Parse(expectedOutput);

        var result = NamedUuid.Create(ns, name);
        await Assert.That(result)
            .IsEqualTo(exp);
    }
}
