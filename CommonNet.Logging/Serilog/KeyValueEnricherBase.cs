using CommunityToolkit.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Serilog;

/// <summary>
/// Base class for KeyValue properties enricher.
/// </summary>
public abstract class KeyValueEnricherBase : ILogEventEnricher
{
    LogEventProperty? _cachedProperty;

    /// <summary>
    /// Property name.
    /// </summary>
    protected string PropertyName { get; }

    /// <summary>
    /// Base class constructor.
    /// </summary>
    /// <param name="name">Propetry name.</param>
    protected KeyValueEnricherBase(string name)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        PropertyName = name;
    }

    /// <summary>
    /// Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        Guard.IsNotNull(logEvent);
        Guard.IsNotNull(propertyFactory);

        logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
    }

    /// <summary>
    /// Gets or creates log event property.
    /// </summary>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    /// <returns>Log event property.</returns>
    private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
    {
        _cachedProperty ??= CreateProperty(propertyFactory);
        return _cachedProperty;
    }

    /// <summary>
    /// Create the log event property.
    /// </summary>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    /// <returns>Log event property.</returns>
    protected abstract LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory);
}
