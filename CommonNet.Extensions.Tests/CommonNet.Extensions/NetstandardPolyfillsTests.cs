#if NET48

using System.Runtime.Versioning;

namespace CommonNet.Extensions.Tests;

public class NetstandardPolyfillsTests
{
    public record PlatformAttributeTestData(OSPlatformAttribute Attribute, string PlatformName);

    public static IEnumerable<PlatformAttributeTestData> GetPlatformAttributesTestData()
    {
        yield return new(new TargetPlatformAttribute("windows"), "windows");
        yield return new(new SupportedOSPlatformAttribute("linux"), "linux");
        yield return new(new UnsupportedOSPlatformAttribute("windows"), "windows");
        yield return new(new UnsupportedOSPlatformAttribute("windows", "Some Message"), "windows");
        yield return new(new ObsoletedOSPlatformAttribute("windows"), "windows");
        yield return new(new ObsoletedOSPlatformAttribute("windows", "Some Message"), "windows");
        yield return new(new SupportedOSPlatformGuardAttribute("linux"), "linux");
        yield return new(new UnsupportedOSPlatformGuardAttribute("windows"), "windows");
    }


    [Test]
    [MethodDataSource(nameof(GetPlatformAttributesTestData))]
    public async Task VersioningAttributesCoverageTest(PlatformAttributeTestData values)
    {
        var (attr, expectedPlatform) = values;
        await Assert.That(attr.PlatformName)
            .IsEqualTo(expectedPlatform);
        if (attr is UnsupportedOSPlatformAttribute uAttr)
        {
            _ = uAttr.Message;
        }
        else if (attr is ObsoletedOSPlatformAttribute oAttr)
        {
            _ = oAttr.Message;
            _ = oAttr.Url;
        }
    }
}

#endif
