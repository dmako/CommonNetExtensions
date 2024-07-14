using System.Net.Sockets;

namespace System.Net.NetworkInformation;

/// <summary>
/// Provides helper methods for working with network interfaces, including filtering and enumeration of active interfaces and addresses.
/// This class is particularly useful for identifying network interfaces and IP addresses that are safe to use in development environments,
/// filtering out interfaces that are less likely to be relevant or useful (e.g., virtual network interfaces, modem connections).
/// Some of the interfaces might be in conflict on development machine with ports binding, like bluetooth or VPN interfaces.
/// </summary>
public static class NetworkInterfaceHelpers
{
    private static readonly NetworkInterfaceType[] DevelopmentEnvironmentDisallowedInterfaceTypes =
    [
        // filter out possible network that might appear during development phase
        NetworkInterfaceType.GenericModem,
        // IEEE1394
        NetworkInterfaceType.HighPerformanceSerialBus,
        NetworkInterfaceType.Isdn,
        NetworkInterfaceType.Ppp,
        NetworkInterfaceType.MultiRateSymmetricDsl,
        NetworkInterfaceType.PrimaryIsdn,
        NetworkInterfaceType.RateAdaptDsl,
        NetworkInterfaceType.Slip,
        NetworkInterfaceType.GenericModem,
        NetworkInterfaceType.TokenRing,
        NetworkInterfaceType.Tunnel,
        NetworkInterfaceType.VeryHighSpeedDsl,
        NetworkInterfaceType.Wman,
        NetworkInterfaceType.Wwanpp,
        NetworkInterfaceType.Wwanpp2,
        // Win NDIS: IF_TYPE_PROP_VIRTUAL, Proprietary virtual/internal, e.g. OpenVPN TAP driver
        (NetworkInterfaceType)53,
        // Win NDIS: IF_TYPE_USB
        (NetworkInterfaceType)160,
    ];

    private static IEnumerable<NetworkInterface> FilterListeningInterfaces(this IEnumerable<NetworkInterface> interfaces)
    {
        return interfaces
            .Where(
                ni => ni.OperationalStatus == OperationalStatus.Up &&
                ni.Supports(NetworkInterfaceComponent.IPv4) &&
                !DevelopmentEnvironmentDisallowedInterfaceTypes.Contains(ni.NetworkInterfaceType) &&
#if NET6_0_OR_GREATER
                !ni.Name.Contains("bluetooth", StringComparison.InvariantCultureIgnoreCase)
#else
                !ni.Name.Contains("bluetooth")
#endif
            );
    }

    /// <summary>
    /// Enumerates the properties of active network interfaces.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{IPInterfaceProperties}"/> containing the properties of all active network interfaces, ordered by descending speed.</returns>
    /// <remarks>
    /// This method filters network interfaces to include only those that are currently up. It is useful for obtaining detailed information about the network interfaces that are active and available for communication.
    /// </remarks>
    public static IEnumerable<IPInterfaceProperties> EnumerateActiveInterfacesProperties()
    {
        return NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(x => x.OperationalStatus == OperationalStatus.Up)
            .OrderByDescending(c => c.Speed)
            .Select(x => x.GetIPProperties());
    }

    /// <summary>
    /// Enumerates the properties of network interfaces that are considered safe for development environments.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{IPInterfaceProperties}"/> containing the properties of all active network interfaces deemed safe for development, ordered by descending speed.</returns>
    /// <remarks>
    /// This method filters out network interfaces based on operational status, support for IPv4, and a predefined list of disallowed interface types that are less relevant or potentially conflicting in development environments (e.g., virtual network interfaces, modem connections).
    /// It is designed to help developers identify network interfaces and IP addresses that are most likely to be suitable for development purposes, reducing the chance of conflicts with system or virtual interfaces.
    /// </remarks>
    public static IEnumerable<IPInterfaceProperties> EnumerateDevelopmentSafeActiveInterfacesProperties()
    {

        return NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(x => x.OperationalStatus == OperationalStatus.Up)
            .FilterListeningInterfaces()
            .OrderByDescending(c => c.Speed)
            .Select(x => x.GetIPProperties());
    }
    /// <summary>
    /// Enumerates all IPv4 addresses from network interfaces that are considered safe for development environments.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{IPAddress}"/> containing all IPv4 addresses from active network interfaces deemed safe for development.</returns>
    /// <remarks>
    /// This method leverages <see cref="EnumerateDevelopmentSafeActiveInterfacesProperties"/> to filter network interfaces before extracting their IPv4 addresses.
    /// It is particularly useful for identifying local IP addresses that are most likely to be suitable for development purposes, avoiding potential conflicts with system or virtual interfaces.
    /// </remarks>
    public static IEnumerable<IPAddress> GetDevelopmentSafeActiveIPv4Addresses()
    {
        foreach (var props in EnumerateDevelopmentSafeActiveInterfacesProperties())
        {
            var ips = props.UnicastAddresses
                .Where(item => item.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(item => item.Address);
            foreach (var ip in ips)
            {
                yield return ip;
            }
        }
    }
}
