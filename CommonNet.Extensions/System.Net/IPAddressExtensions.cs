using System.Net.NetworkInformation;
using System.Net.Sockets;
using CommunityToolkit.Diagnostics;

namespace System.Net;

/// <summary>
/// Provides extension methods for <see cref="IPAddress"/>.
/// </summary>
public static class IPAddressExtensions
{
    /// <summary>
    /// Applies a subnet mask to an IP address.
    /// </summary>
    /// <param name="self">The IP address to be masked.</param>
    /// <param name="mask">The subnet mask to apply.</param>
    /// <returns>A new <see cref="IPAddress"/> that represents the IP address after applying the subnet mask.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="self"/> or <paramref name="mask"/> is null.</exception>
    public static IPAddress Mask(this IPAddress self, IPAddress mask)
    {
        Guard.IsNotNull(self);
        Guard.IsNotNull(mask);


        var src = self.GetAddressBytes();
        var m = mask.GetAddressBytes();
        for (var i = 0; i < src.Length; i++)
        {
            src[i] &= m[i];
        }
        return new IPAddress(src);
    }

    /// <summary>
    /// Gets the local IP address that is on the same network as the provided IP address.
    /// </summary>
    /// <param name="self">The IP address to find a matching local address for.</param>
    /// /// <param name="interfaces">An enumerable of <see cref="IPInterfaceProperties"/> for network interfaces.</param>
    /// <returns>The local <see cref="IPAddress"/> that is on the same network as the provided IP address.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no matching local address is found.</exception>
    public static IPAddress GetLocalAddressOnTheSameNetwork(this IPAddress self, IEnumerable<IPInterfaceProperties> interfaces)
    {
        var selfBytes = self.GetAddressBytes();

        foreach (var interfaceProps in interfaces)
        {
            foreach (var ipInformation in interfaceProps.UnicastAddresses)
            {
                if (ipInformation.Address.AddressFamily.IsOneOf(AddressFamily.InterNetwork, AddressFamily.InterNetworkV6))
                {
                    if (ipInformation.Address.AddressFamily == AddressFamily.InterNetwork && self.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (ipInformation.Address.Mask(ipInformation.IPv4Mask).Equals(self.Mask(ipInformation.IPv4Mask)))
                        {
                            return ipInformation.Address;
                        }
                    }
                    else if (self.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        var ipBytes = ipInformation.Address.GetAddressBytes();
                        var fullBytes = ipInformation.PrefixLength / 8;
                        var remainingBits = ipInformation.PrefixLength % 8;

                        var isMatch = true;

                        for (var i = 0; i < fullBytes; i++)
                        {
                            if (ipBytes[i] != selfBytes[i])
                            {
                                isMatch = false;
                                break;
                            }
                        }
                        if (isMatch && remainingBits > 0)
                        {
                            var mask = (byte)(0xFF << (8 - remainingBits));
                            if ((ipBytes[fullBytes] & mask) == (selfBytes[fullBytes] & mask))
                            {
                                return ipInformation.Address;
                            }
                        }
                        if (isMatch)
                        {
                            return ipInformation.Address;
                        }
                    }
                }
            }
        }
        throw new InvalidOperationException("Failed to find local address");
    }
}
