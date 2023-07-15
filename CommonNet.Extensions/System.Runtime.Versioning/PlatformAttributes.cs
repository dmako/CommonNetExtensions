#if NETSTANDARD2_0

namespace System.Runtime.Versioning;

/// <summary>
/// Base polyfill type for all platform attributes.
/// </summary>
public abstract class OSPlatformAttribute : Attribute
{
    private protected OSPlatformAttribute(string platformName)
    {
        PlatformName = platformName;
    }

    /// <summary>
    /// Platform name
    /// </summary>
    public string PlatformName { get; }
}

/// <summary>
/// Polyfill attribute for the platform that the project targeted.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class TargetPlatformAttribute : OSPlatformAttribute
{
    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    public TargetPlatformAttribute(string platformName)
        : base(platformName)
    {
    }
}

/// <summary>
/// Polyfill attribute for the operating system that API supports.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly |
                AttributeTargets.Class |
                AttributeTargets.Constructor |
                AttributeTargets.Enum |
                AttributeTargets.Event |
                AttributeTargets.Field |
                AttributeTargets.Interface |
                AttributeTargets.Method |
                AttributeTargets.Module |
                AttributeTargets.Property |
                AttributeTargets.Struct,
                AllowMultiple = true, Inherited = false)]
public sealed class SupportedOSPlatformAttribute : OSPlatformAttribute
{
    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    public SupportedOSPlatformAttribute(string platformName)
        : base(platformName)
    {
    }
}

/// <summary>
/// Polyfill attribute for the operating system that API don't supports.
/// </summary>

[AttributeUsage(AttributeTargets.Assembly |
                AttributeTargets.Class |
                AttributeTargets.Constructor |
                AttributeTargets.Enum |
                AttributeTargets.Event |
                AttributeTargets.Field |
                AttributeTargets.Interface |
                AttributeTargets.Method |
                AttributeTargets.Module |
                AttributeTargets.Property |
                AttributeTargets.Struct,
                AllowMultiple = true, Inherited = false)]
public sealed class UnsupportedOSPlatformAttribute : OSPlatformAttribute
{
    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    public UnsupportedOSPlatformAttribute(string platformName)
        : base(platformName)
    {
    }

    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    /// <param name="message">Message.</param>
    public UnsupportedOSPlatformAttribute(string platformName, string? message)
        : base(platformName)
    {
        Message = message;
    }
    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; }
}

/// <summary>
/// Polyfill attribute for marking the API as obsoleted for the operating system.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly |
                AttributeTargets.Class |
                AttributeTargets.Constructor |
                AttributeTargets.Enum |
                AttributeTargets.Event |
                AttributeTargets.Field |
                AttributeTargets.Interface |
                AttributeTargets.Method |
                AttributeTargets.Module |
                AttributeTargets.Property |
                AttributeTargets.Struct,
                AllowMultiple = true, Inherited = false)]
public sealed class ObsoletedOSPlatformAttribute : OSPlatformAttribute
{
    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    public ObsoletedOSPlatformAttribute(string platformName)
        : base(platformName)
    {
    }

    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    /// <param name="message">Message.</param>
    public ObsoletedOSPlatformAttribute(string platformName, string? message)
        : base(platformName)
    {
        Message = message;
    }

    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// Url
    /// </summary>
    public string? Url { get; set; }
}

/// <summary>
/// Polyfill attribute for marking the member with supported operating system guard.
/// </summary>
[AttributeUsage(AttributeTargets.Field |
                AttributeTargets.Method |
                AttributeTargets.Property,
                AllowMultiple = true, Inherited = false)]
public sealed class SupportedOSPlatformGuardAttribute : OSPlatformAttribute
{
    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    public SupportedOSPlatformGuardAttribute(string platformName)
        : base(platformName)
    {
    }
}

/// <summary>
/// Polyfill attribute for marking the member with unsupported operating system guard.
/// </summary>
[AttributeUsage(AttributeTargets.Field |
                AttributeTargets.Method |
                AttributeTargets.Property,
                AllowMultiple = true, Inherited = false)]
public sealed class UnsupportedOSPlatformGuardAttribute : OSPlatformAttribute
{
    /// <summary>
    /// Polyfill Constructor
    /// </summary>
    /// <param name="platformName">Name of the platform.</param>
    public UnsupportedOSPlatformGuardAttribute(string platformName)
        : base(platformName)
    {
    }
}

#endif
