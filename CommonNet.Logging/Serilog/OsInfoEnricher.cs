﻿using System.Runtime.InteropServices;
using CommunityToolkit.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Serilog;

/// <summary>
/// Creates log event property containing the information about OS the application is running on.
/// </summary>
public class OsInfoEnricher : KeyValueEnricherBase
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public OsInfoEnricher()
        : base ("OsInfo")
    {
    }

    /// <summary>
    /// Create the log event property.
    /// </summary>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    /// <returns>Log event property.</returns>
    protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
    {
        Guard.IsNotNull(propertyFactory);
        return propertyFactory.CreateProperty(PropertyName, RuntimeInformation.OSDescription);
    }
}
