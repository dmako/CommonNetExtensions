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
            Action act = () => targetIPAddress.GetLocalAddressOnTheSameNetwork(interfaces);
            await Assert.That(act)
                .ThrowsExactly<InvalidOperationException>()
                .WithMessage("Failed to find local address");
        }
    }
}
