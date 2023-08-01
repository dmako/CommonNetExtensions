using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using CommunityToolkit.Diagnostics;

namespace System;

/// <summary>
/// Name-Based UUID extensions to create UUIDs as stated in RFC 4122 4.3
/// </summary>
public static class NamedUuid
{
    /// <summary>
    /// FQNs Namespace (RFC 4122 Appendix C).
    /// </summary>
    public static readonly Guid FqnNamespace = new("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

    /// <summary>
    /// URLs Namespace (RFC 4122 Appendix C).
    /// </summary>
    public static readonly Guid UrlNamespace = new("6ba7b811-9dad-11d1-80b4-00c04fd430c8");

    /// <summary>
    /// ISO OIDs Namespace (RFC 4122 Appendix C).
    /// </summary>
    public static readonly Guid IsoOidNamespace = new("6ba7b812-9dad-11d1-80b4-00c04fd430c8");

    /// <summary>
    /// X.500 DNs Namespace (RFC 4122 Appendix C).
    /// </summary>
    public static readonly Guid X500DnNamespace = new("6ba7b814-9dad-11d1-80b4-00c04fd430c8");

    /// <summary>
    /// Creates a name-based UUID
    /// </summary>
    /// <param name="namespaceId">The ID of the namespace.</param>
    /// <param name="name">The name within namespace.</param>
    /// <returns>A UUID derived from the namespace and name.</returns>
    public static Guid Create(Guid namespaceId, string name)
    {
        Guard.IsNotNullOrEmpty(name);
        // Convert the name to a canonical sequence of octets (as defined by the standards or conventions of its name space)
        return Create(namespaceId, Encoding.UTF8.GetBytes(name));
    }

    /// <summary>
    /// Creates a name-based UUID
    /// </summary>
    /// <param name="namespaceId">The ID of the namespace.</param>
    /// <param name="nameBytes">The name within namespace.</param>
    /// <returns>A UUID derived from the namespace and name.</returns>
    public static Guid Create(Guid namespaceId, byte[] nameBytes)
    {
        // put the namespace ID in network byte order.
        var namespaceBytes = namespaceId.ToByteArray();
        SwapNetworkByteOrder(namespaceBytes);

        // Compute the hash of the name space ID concatenated with the name.
        var data = namespaceBytes.Concat(nameBytes).ToArray();
#if NETSTANDARD2_0
        using var algorithm = SHA1.Create();
        var hash = algorithm.ComputeHash(data);
#else
        var hash = SHA1.HashData(data);
#endif
        var newGuid = new byte[16];

        // 0                   1                   2                   3
        //  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                          time_low                             |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |       time_mid                |         time_hi_and_version   |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |clk_seq_hi_res |  clk_seq_low  |         node (0-1)            |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        // |                         node (2-5)                            |
        // +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

        // Set octets zero through 3 of the time_low field to octets zero through 3 of the hash.
        // Set octets zero and one of the time_mid field to octets 4 and 5 of the hash.
        // Set octets zero and one of the time_hi_and_version field to octets 6 and 7 of the hash.
        // Set the clock_seq_hi_and_reserved field to octet 8 of the hash.
        // Set the clock_seq_low field to octet 9 of the hash.
        // Set octets zero through five of the node field to octets 10 through 15 of the hash.
        hash.AsReadOnlySpan()[..newGuid.Length].CopyTo(newGuid);

        // mark it as V5 - The name-based version that uses SHA-1 hashing.
        // Set the four most significant bits (bits 12 through 15) of the time_hi_and_version field to the appropriate 4-bit version number from Section 4.1.3.
        newGuid[6] = (byte)((newGuid[6] & 0x0F) | (5 << 4));
        // Set octets zero and one of the time_hi_and_version field to octets 6 and 7 of the hash.
        newGuid[8] = (byte)((newGuid[8] & 0x3F) | 0x80);

        // Convert the resulting UUID to local byte order.
        SwapNetworkByteOrder(newGuid);
        return new Guid(newGuid);
    }

    private static void SwapNetworkByteOrder(Span<byte> guid)
    {
        SwapArrayBytes(guid, 0, 3);
        SwapArrayBytes(guid, 1, 2);
        SwapArrayBytes(guid, 4, 5);
        SwapArrayBytes(guid, 6, 7);
    }

    private static void SwapArrayBytes(Span<byte> guid, int leftIdx, int rightIdx)
    {
        (guid[rightIdx], guid[leftIdx]) = (guid[leftIdx], guid[rightIdx]);
    }
}
