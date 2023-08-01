using CommunityToolkit.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Serilog;

/// <summary>
/// Creates log event property containing the information about the installation location.
/// </summary>
public class LocationEnricher : KeyValueEnricherBase
{
    private readonly string _locationName;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="locationName">The location name</param>
    public LocationEnricher(string locationName = "")
        : base("Location")
    {
        _locationName = string.IsNullOrWhiteSpace(locationName) ?
            Environment.MachineName :
            locationName;
    }

    /// <summary>
    /// Create the log event property.
    /// </summary>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    /// <returns>Log event property.</returns>
    protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
    {
        Guard.IsNotNull(propertyFactory);

        return propertyFactory.CreateProperty(PropertyName, _locationName);
    }
}
