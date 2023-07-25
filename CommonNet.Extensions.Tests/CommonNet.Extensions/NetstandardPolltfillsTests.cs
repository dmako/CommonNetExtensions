#if NET48

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class NetstandardPolltfillsTests
{
    public static List<object[]> PlatformAttributesTestData =>
        new()
        {
            new object[] { new TargetPlatformAttribute("windows"), "windows" },
            new object[] { new SupportedOSPlatformAttribute("linux"), "linux" },
            new object[] { new UnsupportedOSPlatformAttribute("windows"), "windows" },
            new object[] { new UnsupportedOSPlatformAttribute("windows", "Some Message"), "windows" },
            new object[] { new ObsoletedOSPlatformAttribute("windows"), "windows" },
            new object[] { new ObsoletedOSPlatformAttribute("windows", "Some Message"), "windows" },
            new object[] { new SupportedOSPlatformGuardAttribute("linux"), "linux" },
            new object[] { new UnsupportedOSPlatformGuardAttribute("windows"), "windows" }
        };



    [Theory]
    [MemberData(nameof(PlatformAttributesTestData))]
    public void VersioningAttributesCoverageTest(OSPlatformAttribute attr, string expectedPlatform)
    {
        attr.PlatformName.Should().Be(expectedPlatform);
        if (attr is UnsupportedOSPlatformAttribute uattr)
        {
            // to not optimize out on release configuration
            uattr.Message?.Should().NotBeNull(expectedPlatform);
        }
        else if (attr is ObsoletedOSPlatformAttribute oattr)
        {
            // to not optimize out on release configuration
            oattr.Message?.Should().NotBeNull(expectedPlatform);
            oattr.Url.Should().BeNull();
        }
    }

}

#endif
