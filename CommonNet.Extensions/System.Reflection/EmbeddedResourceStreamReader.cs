using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using CommunityToolkit.Diagnostics;

namespace System.Reflection;

/// <summary>
/// Extension methods for working with <see cref="Assembly"/> embedded manifest stream.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class EmbeddedResourceStreamExtensions
{
    private static string ExpandResourceName(this Assembly assembly, string resourceName)
    {
        var manifestResourceNames = assembly.GetManifestResourceNames();
        if (manifestResourceNames.Any(name => name.EndsWith(resourceName)))
        {
            return manifestResourceNames.Single(str => str.EndsWith(resourceName));
        }
        return resourceName;
    }

    /// <summary>
    /// Get Embedded Resource Stream from assembly.
    /// </summary>
    /// <param name="assembly">Assembly that contains the embedded resource stream.</param>
    /// <param name="resourceName">Full or partial name of the resource.</param>
    /// <returns>The embedded stream.</returns>
    public static Stream GetEmbeddedResourceStream(this Assembly assembly, string resourceName)
    {
        Guard.IsNotNull(assembly);
        Guard.IsNotNullOrWhiteSpace(resourceName);

        resourceName = ExpandResourceName(assembly, resourceName);
        return assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Read Embedded Resource Stream data from assembly.
    /// </summary>
    /// <param name="assembly">Assembly that contains the embedded resource stream.</param>
    /// <param name="resourceName"></param>
    /// <returns>The embedded stream data.</returns>
    public static byte[] ReadEmbeddedResourceData(this Assembly assembly, string resourceName)
    {
        Guard.IsNotNull(assembly);

        using var stream = assembly.GetEmbeddedResourceStream(resourceName);
        Guard.IsNotNull(stream, nameof(resourceName));

        var data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        return data;
    }

    /// <summary>
    /// Iterate lines of embedded resource stream.
    /// </summary>
    /// <param name="assembly">Assembly that contains the embedded resource stream.</param>
    /// <param name="resourceName">Full or partial name of the resource.</param>
    /// <param name="encoding">Encoding of embedded stream text content. If not provided UTF-8 is used.</param>
    /// <returns>Enumerable </returns>
    public static IEnumerable<string> ReadEmbeddedResourceLines(this Assembly assembly, string resourceName, Encoding? encoding = null)
    {
        Guard.IsNotNull(assembly);

        using var stream = assembly.GetEmbeddedResourceStream(resourceName);
        Guard.IsNotNull(stream, nameof(resourceName));

        encoding ??= Encoding.UTF8;
        using var reader = new StreamReader(stream, encoding);

        foreach (var line in reader.EnumLines())
        {
            yield return line;
        }
    }

    /// <summary>
    /// Iterate lines of embedded resource stream.
    /// </summary>
    /// <param name="assembly">Assembly that contains the embedded resource stream.</param>
    /// <param name="resourceName">Full or partial name of the resource.</param>
    /// <param name="encoding">Encoding of embedded stream text content. If not provided UTF-8 is used.</param>
    /// /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>Enumerable </returns>
    public static async IAsyncEnumerable<string> ReadEmbeddedResourceLinesAsync(
        this Assembly assembly,
        string resourceName,
        Encoding? encoding = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        Guard.IsNotNull(assembly);

        using var stream = assembly.GetEmbeddedResourceStream(resourceName);
        Guard.IsNotNull(stream, nameof(resourceName));

        encoding ??= Encoding.UTF8;
        using var reader = new StreamReader(stream, encoding);

        await foreach(var line in reader.EnumLinesAsync(cancellationToken))
        {
            yield return line;
        } 
    }
}
