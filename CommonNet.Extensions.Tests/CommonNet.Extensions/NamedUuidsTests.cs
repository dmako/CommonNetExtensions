using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class NamedUuidsTests
{
    public static readonly List<object[]> NamedUuidGenerationTestData = new()
    {
        new object[] { NamedUuid.IsoOidNamespace, "1.3.6.1.4.1.343" },
        new object[] { NamedUuid.UrlNamespace, "https://wikipedia.org/" },
        new object[] { NamedUuid.FqnNamespace, "my.computer.org" },
        new object[] { NamedUuid.X500DnNamespace, "C=US;O=Example Organisation;CN=Test User 1" },
        new object[] { NamedUuid.X500DnNamespace, "CN=My User;O=My Org;OU=Unit;C=AU;L=My Town;S=NSW;E=myuser@my.org" },
        new object[] { Guid.Parse("00000000-dead-beef-f00d-000000000000"), "Private namespace and identificator" }
    };


    [Theory]
    [MemberData(nameof(NamedUuidGenerationTestData))]
    public void Create_ShouldProduceSameOutput_WithSameInput(Guid namespaceId, string name)
    {
        var uuid1= NamedUuid.Create(namespaceId, name);
        var uuid2 = NamedUuid.Create(namespaceId, name);
        uuid1.Should().Be(uuid2);
    }

    [Theory]
    [InlineData("6ba7b810-9dad-11d1-80b4-00c04fd430c8", "www.example.org", "74738ff5-5367-5958-9aee-98fffdcd1876")]
    [InlineData("b08bfdda-de67-4bad-b2d3-b6bb3a747761", "exmaple name", "f5f17fe3-4176-563e-b0e8-0c2f41cc8fbd")]
    [InlineData("6ba7b810-9dad-11d1-80b4-00c04fd430c8", "my.computer.org", "3aa8cd3e-12cb-5d4f-9ad4-ee0774b4b93d")]
    public void Create_ShouldProduceSameOutput_BasedOnTestVectorsFromInternet(string namespaceId, string name, string expectedOutput)
    {
        var ns = Guid.Parse(namespaceId);
        var exp = Guid.Parse(expectedOutput);

        var result = NamedUuid.Create(ns, name);
        result.Should().Be(exp);
    }
}
