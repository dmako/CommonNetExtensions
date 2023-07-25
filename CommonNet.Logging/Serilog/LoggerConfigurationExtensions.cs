using CommunityToolkit.Diagnostics;
using Serilog.Configuration;

namespace Serilog;

/// <summary>
/// Extends <see cref="LoggerConfiguration"/> to add enrichers.
/// </summary>
public static class LoggerConfigurationExtensions
{
    /// <summary>
    /// Enrich log events with a OsInfo property containing the information about OS the application is running on.
    /// </summary>
    /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
    /// <returns>Configuration object allowing method chaining.</returns>
    public static LoggerConfiguration WithOsInfo(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        Guard.IsNotNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With<OsInfoEnricher>();
    }

    /// <summary>
    /// Enrich log events with a FrameworkVersion property containing the information about .Net runtime the application is running on.
    /// </summary>
    /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
    /// <returns>Configuration object allowing method chaining.</returns>
    public static LoggerConfiguration WithFrameworkVersion(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        Guard.IsNotNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With<FrameworkVersionEnricher>();
    }

    /// <summary>
    /// Enrich log events with a InstallationEnvironment property containing the information about the installation environment.
    /// </summary>
    /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
    /// <param name="envName">Environment name to add to property. Default is environment name of process.</param>
    /// <returns>Configuration object allowing method chaining.</returns>
    public static LoggerConfiguration WithInstallationEnvironmentName(this LoggerEnrichmentConfiguration enrichmentConfiguration, string envName = "")
    {
        Guard.IsNotNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With(new InstallationEnvironmentName(envName));
    }

    /// <summary>
    /// Enrich log events with a Location property containing the information about the installation location.
    /// </summary>
    /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
    /// <param name="locationName">Location name to add to property. Default is machine name.</param>
    /// <returns>Configuration object allowing method chaining.</returns>
    public static LoggerConfiguration WithLocation(this LoggerEnrichmentConfiguration enrichmentConfiguration, string locationName = "")
    {
        Guard.IsNotNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With(new LocationEnricher(locationName));
    }
}
