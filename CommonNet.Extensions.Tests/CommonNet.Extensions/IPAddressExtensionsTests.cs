using System.Net;
using System.Net.NetworkInformation;
using Moq;

namespace CommonNet.Extensions.Tests;

public class IPAddressExtensionsTests
{
    [Test]
    [Arguments("192.168.1.1", "255.255.255.0", "192.168.1.0")]
    [Arguments("172.16.0.1", "255.255.0.0", "172.16.0.0")]
    [Arguments("10.0.0.1", "255.0.0.0", "10.0.0.0")]
    public async Task Mask_ShouldCorrectlyApplySubnetMask(string ipAddress, string mask, string expected)
    {
        var ip = IPAddress.Parse(ipAddress);
        var subnetMask = IPAddress.Parse(mask);
        var expectedResult = IPAddress.Parse(expected);

        var result = ip.Mask(subnetMask);

        await Assert.That(result)
            .EqualTo(expectedResult);
    }

    [Test]
    public async Task Mask_ShouldThrowArgumentNullException_WhenIpAddressIsNull()
    {
        IPAddress ip = null!;
        var subnetMask = IPAddress.Parse("255.255.255.0");
        await Assert.That(() => ip.Mask(subnetMask))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("self");
    }

    [Test]
    public async Task Mask_ShouldThrowArgumentNullException_WhenSubnetMaskIsNull()
    {
        var ip = IPAddress.Parse("192.168.1.1");
        IPAddress subnetMask = null!;
        
        await Assert.That(() => ip.Mask(subnetMask))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("mask");
    }

    [Test]
    public async Task Mask_ShouldHandleIPv6Addresses()
    {
        var ip = IPAddress.Parse("fe80::d503:4ee:3882:c586");
        var subnetMask = IPAddress.Parse("ffff:ffff:ffff:ffff::");
        var expected = IPAddress.Parse("fe80::");

        var result = ip.Mask(subnetMask);

        await Assert.That(result)
            .EqualTo(expected);
    }

    [Test]
    public async Task Mask_AllZerosMask_ShouldReturnAllZerosAddress()
    {
        var ip = IPAddress.Parse("192.168.1.100");
        var mask = IPAddress.Parse("0.0.0.0");
        var expected = IPAddress.Parse("0.0.0.0");

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(expected);
    }

    [Test]
    public async Task Mask_AllOnesMask_ShouldReturnOriginalAddress()
    {
        var ip = IPAddress.Parse("192.168.1.100");
        var mask = IPAddress.Parse("255.255.255.255");

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(ip);
    }

    [Test]
    [Arguments("192.168.1.200", "255.255.255.128", "192.168.1.128")]
    [Arguments("192.168.1.50", "255.255.255.128", "192.168.1.0")]
    [Arguments("10.10.10.10", "255.255.255.192", "10.10.10.0")]
    [Arguments("10.10.10.200", "255.255.255.192", "10.10.10.192")]
    public async Task Mask_NonOctetAlignedMask_ShouldMaskCorrectly(string ipAddress, string maskAddress, string expected)
    {
        var ip = IPAddress.Parse(ipAddress);
        var mask = IPAddress.Parse(maskAddress);
        var expectedResult = IPAddress.Parse(expected);

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(expectedResult);
    }

    [Test]
    public async Task Mask_BroadcastAddress_ShouldReturnNetworkAddress()
    {
        var ip = IPAddress.Parse("255.255.255.255");
        var mask = IPAddress.Parse("255.255.255.0");
        var expected = IPAddress.Parse("255.255.255.0");

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(expected);
    }

    [Test]
    public async Task Mask_ZeroAddress_ShouldReturnZeroAddress()
    {
        var ip = IPAddress.Parse("0.0.0.0");
        var mask = IPAddress.Parse("255.255.255.0");
        var expected = IPAddress.Parse("0.0.0.0");

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(expected);
    }

    [Test]
    public async Task Mask_IPv6_AllZerosMask_ShouldReturnAllZeros()
    {
        var ip = IPAddress.Parse("fe80::d503:4ee:3882:c586");
        var mask = IPAddress.Parse("::");
        var expected = IPAddress.Parse("::");

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(expected);
    }

    [Test]
    public async Task Mask_IPv6_AllOnesMask_ShouldReturnOriginalAddress()
    {
        var ip = IPAddress.Parse("fe80::d503:4ee:3882:c586");
        var mask = IPAddress.Parse("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff");

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(ip);
    }

    [Test]
    public async Task Mask_IPv6_NonAlignedPrefixMask_ShouldMaskCorrectly()
    {
        var ip = IPAddress.Parse("2001:0db8:85a3:0000:0000:8a2e:0370:7334");
        var mask = IPAddress.Parse("ffff:ffff:ffff:ff00::");
        var expected = IPAddress.Parse("2001:0db8:85a3::");

        var result = ip.Mask(mask);

        await Assert.That(result)
            .EqualTo(expected);
    }

    [Test]
    [Arguments("192.168.1.100", "192.168.1.1", "255.255.255.0", true)]
    [Arguments("192.168.2.100", "192.168.1.1", "255.255.255.0", false)]
    public async Task GetLocalIpv4AddressOnTheSameNetwork_ShouldFindMatchingAddress_WhenExists(string localIp, string targetIp, string mask, bool shouldFind)
    {
        var localIPAddress = IPAddress.Parse(localIp);
        var targetIPAddress = IPAddress.Parse(targetIp);
        var subnetMask = IPAddress.Parse(mask);

        var mockUnicastIPAddressInformation = new Mock<UnicastIPAddressInformation>();
        mockUnicastIPAddressInformation.Setup(m => m.Address).Returns(localIPAddress);
        mockUnicastIPAddressInformation.Setup(m => m.IPv4Mask).Returns(subnetMask);

        var mockIPInterfaceProperties = new Mock<IPInterfaceProperties>();
        var unicastCollection = new Mock<UnicastIPAddressInformationCollection>();
        var unicastList = new List<UnicastIPAddressInformation> { mockUnicastIPAddressInformation.Object };
        unicastCollection.Setup(m => m.GetEnumerator()).Returns(unicastList.GetEnumerator);
        unicastCollection.Setup(m => m.Count).Returns(unicastList.Count);
        unicastCollection.Setup(m => m[It.IsAny<int>()]).Returns(unicastList[0]);

        mockIPInterfaceProperties.Setup(m => m.UnicastAddresses).Returns(unicastCollection.Object);

        var interfaces = new[] { mockIPInterfaceProperties.Object };

        if (shouldFind)
        {
            var result = targetIPAddress.GetLocalAddressOnTheSameNetwork(interfaces);
            await Assert.That(result)
                .EqualTo(localIPAddress);
        }
        else
        {
            await Assert.That(() => targetIPAddress.GetLocalAddressOnTheSameNetwork(interfaces))
                .ThrowsExactly<InvalidOperationException>()
                .WithMessage("Failed to find local address");
        }
    }

    [Test]
    [Arguments("fe80::d503:4ee:3882:c586", "fe80::d503:4ee:3882:c587", 14 * 8, true)]
    [Arguments("fe80::d504:4ee:3882:c586", "fe80::d503:4ee:3883:c587", 14 * 8, false)]
    public async Task GetLocalIpv6AddressOnTheSameNetwork_ShouldFindMatchingAddress_WhenExists(string localIp, string targetIp, int prefixLength, bool shouldFind)
    {
        var localIPAddress = IPAddress.Parse(localIp);
        var targetIPAddress = IPAddress.Parse(targetIp);

        var mockUnicastIPAddressInformation = new Mock<UnicastIPAddressInformation>();
        mockUnicastIPAddressInformation.Setup(m => m.Address).Returns(localIPAddress);
        mockUnicastIPAddressInformation.Setup(m => m.PrefixLength).Returns(prefixLength);

        var mockIPInterfaceProperties = new Mock<IPInterfaceProperties>();
        var unicastCollection = new Mock<UnicastIPAddressInformationCollection>();
        var unicastList = new List<UnicastIPAddressInformation> { mockUnicastIPAddressInformation.Object };
        unicastCollection.Setup(m => m.GetEnumerator()).Returns(unicastList.GetEnumerator);
        unicastCollection.Setup(m => m.Count).Returns(unicastList.Count);
        unicastCollection.Setup(m => m[It.IsAny<int>()]).Returns(unicastList[0]);

        mockIPInterfaceProperties.Setup(m => m.UnicastAddresses).Returns(unicastCollection.Object);

        var interfaces = new[] { mockIPInterfaceProperties.Object };

        if (shouldFind)
        {
            var result = targetIPAddress.GetLocalAddressOnTheSameNetwork(interfaces);
            await Assert.That(result)
                .EqualTo(localIPAddress);
        }
        else
        {
            await Assert.That(() => targetIPAddress.GetLocalAddressOnTheSameNetwork(interfaces))
                .ThrowsExactly<InvalidOperationException>()
                .WithMessage("Failed to find local address");
        }
    }

    // ===== New GetLocalAddressOnTheSameNetwork tests =====

    private static Mock<UnicastIPAddressInformation> CreateIPv4UnicastMock(string address, string mask)
    {
        var mock = new Mock<UnicastIPAddressInformation>();
        mock.Setup(m => m.Address).Returns(IPAddress.Parse(address));
        mock.Setup(m => m.IPv4Mask).Returns(IPAddress.Parse(mask));
        return mock;
    }

    private static Mock<UnicastIPAddressInformation> CreateIPv6UnicastMock(string address, int prefixLength)
    {
        var mock = new Mock<UnicastIPAddressInformation>();
        mock.Setup(m => m.Address).Returns(IPAddress.Parse(address));
        mock.Setup(m => m.PrefixLength).Returns(prefixLength);
        return mock;
    }

    private static IPInterfaceProperties CreateMockInterface(params Mock<UnicastIPAddressInformation>[] unicastMocks)
    {
        var unicastList = unicastMocks.Select(m => m.Object).ToList();
        var unicastCollection = new Mock<UnicastIPAddressInformationCollection>();
        unicastCollection.Setup(m => m.GetEnumerator()).Returns(unicastList.GetEnumerator);
        unicastCollection.Setup(m => m.Count).Returns(unicastList.Count);

        var mockProps = new Mock<IPInterfaceProperties>();
        mockProps.Setup(m => m.UnicastAddresses).Returns(unicastCollection.Object);
        return mockProps.Object;
    }

    [Test]
    public async Task GetLocalAddressOnTheSameNetwork_EmptyInterfaces_ShouldThrow()
    {
        var target = IPAddress.Parse("192.168.1.1");
        var interfaces = Array.Empty<IPInterfaceProperties>();

        await Assert.That(() => target.GetLocalAddressOnTheSameNetwork(interfaces))
            .ThrowsExactly<InvalidOperationException>()
            .WithMessage("Failed to find local address");
    }

    [Test]
    public async Task GetLocalAddressOnTheSameNetwork_InterfaceWithNoUnicastAddresses_ShouldThrow()
    {
        var target = IPAddress.Parse("192.168.1.1");

        var unicastCollection = new Mock<UnicastIPAddressInformationCollection>();
        var emptyList = new List<UnicastIPAddressInformation>();
        unicastCollection.Setup(m => m.GetEnumerator()).Returns(emptyList.GetEnumerator);
        unicastCollection.Setup(m => m.Count).Returns(0);

        var mockProps = new Mock<IPInterfaceProperties>();
        mockProps.Setup(m => m.UnicastAddresses).Returns(unicastCollection.Object);

        await Assert.That(() => target.GetLocalAddressOnTheSameNetwork([mockProps.Object]))
            .ThrowsExactly<InvalidOperationException>()
            .WithMessage("Failed to find local address");
    }

    [Test]
    public async Task GetLocalAddressOnTheSameNetwork_MultipleIPv4Interfaces_ShouldFindMatchOnSecond()
    {
        var target = IPAddress.Parse("10.0.0.50");
        var iface1 = CreateMockInterface(CreateIPv4UnicastMock("192.168.1.100", "255.255.255.0"));
        var iface2 = CreateMockInterface(CreateIPv4UnicastMock("10.0.0.1", "255.255.255.0"));

        var result = target.GetLocalAddressOnTheSameNetwork([iface1, iface2]);

        await Assert.That(result)
            .EqualTo(IPAddress.Parse("10.0.0.1"));
    }

    [Test]
    public async Task GetLocalAddressOnTheSameNetwork_MultipleUnicastAddresses_ShouldFindMatchOnSecond()
    {
        var target = IPAddress.Parse("10.0.0.50");
        var iface = CreateMockInterface(
            CreateIPv4UnicastMock("192.168.1.100", "255.255.255.0"),
            CreateIPv4UnicastMock("10.0.0.1", "255.255.255.0")
        );

        var result = target.GetLocalAddressOnTheSameNetwork([iface]);

        await Assert.That(result)
            .EqualTo(IPAddress.Parse("10.0.0.1"));
    }

    [Test]
    public async Task GetLocalIPv6AddressOnTheSameNetwork_NonByteAlignedPrefix_ShouldMatch()
    {
        // Prefix /10: fullBytes=1, remainingBits=2
        // Both: byte[0]=0x20 (match), remaining mask=0xC0: byte[1] 0x01 & 0xC0 = 0x00 for both → match
        var target = IPAddress.Parse("2001:db8::1");
        var iface = CreateMockInterface(CreateIPv6UnicastMock("2001:db8::2", 10));

        var result = target.GetLocalAddressOnTheSameNetwork([iface]);

        await Assert.That(result)
            .EqualTo(IPAddress.Parse("2001:db8::2"));
    }

    [Test]
    public async Task GetLocalIPv6AddressOnTheSameNetwork_NonByteAlignedPrefix_FullBytesMismatch_ShouldThrow()
    {
        // Prefix /10: fullBytes=1, remainingBits=2
        // byte[0]: 0x20 vs 0xfe → full byte mismatch → isMatch=false
        var target = IPAddress.Parse("2001:db8::1");
        var iface = CreateMockInterface(CreateIPv6UnicastMock("fe80::1", 10));

        await Assert.That(() => target.GetLocalAddressOnTheSameNetwork([iface]))
            .ThrowsExactly<InvalidOperationException>()
            .WithMessage("Failed to find local address");
    }

    [Test]
    public async Task GetLocalAddressOnTheSameNetwork_IPv4TargetWithIPv6Interface_ShouldThrow()
    {
        // IPv4 target: first if (InterNetwork && InterNetwork) → false (interface is IPv6)
        // else if (InterNetworkV6) → false (self is IPv4)
        // → no match
        var target = IPAddress.Parse("192.168.1.1");
        var iface = CreateMockInterface(CreateIPv6UnicastMock("fe80::1", 64));

        await Assert.That(() => target.GetLocalAddressOnTheSameNetwork([iface]))
            .ThrowsExactly<InvalidOperationException>()
            .WithMessage("Failed to find local address");
    }

    [Test]
    public async Task GetLocalAddressOnTheSameNetwork_IPv6TargetWithIPv4Interface_ShouldThrow()
    {
        // IPv6 target with IPv4 interface: enters else-if (self is IPv6),
        // compares IPv4 bytes (4) with IPv6 bytes (16) → mismatch on full bytes
        var target = IPAddress.Parse("fe80::1");

        var mockUnicast = new Mock<UnicastIPAddressInformation>();
        mockUnicast.Setup(m => m.Address).Returns(IPAddress.Parse("192.168.1.100"));
        mockUnicast.Setup(m => m.IPv4Mask).Returns(IPAddress.Parse("255.255.255.0"));
        mockUnicast.Setup(m => m.PrefixLength).Returns(24);

        var iface = CreateMockInterface(mockUnicast);

        await Assert.That(() => target.GetLocalAddressOnTheSameNetwork([iface]))
            .ThrowsExactly<InvalidOperationException>()
            .WithMessage("Failed to find local address");
    }
}
