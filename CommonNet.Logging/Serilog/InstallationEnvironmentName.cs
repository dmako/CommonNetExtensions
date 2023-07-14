using CommunityToolkit.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Profiprint.Infrastructure.Serilog;

/// <summary>
/// Creates log event property containing the information about the installation environment.
/// </summary>
public class InstallationEnvironmentName : KeyValueEnricherBase
{

    private readonly string _envName;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="envName">The environment name</param>
    public InstallationEnvironmentName(string envName = "")
        : base("InstallationEnvironment")
    {
        _envName = string.IsNullOrWhiteSpace(envName) ?
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production" :
            envName;
    }

    /// <summary>
    /// Create the log event property.
    /// </summary>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    /// <returns>Log event property.</returns>
    protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
    {
        Guard.IsNotNull(propertyFactory, nameof(propertyFactory));

        return propertyFactory.CreateProperty(PropertyName, _envName);
    }
}
