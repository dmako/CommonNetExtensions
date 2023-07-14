using System.Runtime.InteropServices;
using CommunityToolkit.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Profiprint.Infrastructure.Serilog;

/// <summary>
/// Creates log event property containing the information about .Net runtime the application is running on.
/// </summary>
public class FrameworkVersionEnricher : KeyValueEnricherBase
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public FrameworkVersionEnricher()
        : base("FrameworkVersion")
    {
    }

    /// <summary>
    /// Create the log event property.
    /// </summary>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    /// <returns>Log event property.</returns>
    protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
    {
        Guard.IsNotNull(propertyFactory, nameof(propertyFactory));

        return propertyFactory.CreateProperty(PropertyName, RuntimeInformation.FrameworkDescription);
    }
}


