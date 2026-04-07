using System.Runtime.InteropServices;
using Serilog;
using Serilog.Events;

namespace CommonNet.Logging.Tests;

public class EnrichersTests
{
    private static TResult GetPropertyValue<TResult>(LogEvent logEvent, string propertyName)
    {
        return (TResult)(((ScalarValue)logEvent.Properties[propertyName]!).Value!);
    }

    [Test]
    [Arguments("")]
    [Arguments("Switzerland")]
    [Arguments("Czechia")]
    [Arguments("Prague")]
    public async Task LocationEnricher_ShouldBeApplied(string locationName)
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithLocation(locationName)
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        await Assert.That(logEvent)
            .IsNotNull();

        locationName = locationName != "" ? locationName : Environment.MachineName;
        var eventLocationName = GetPropertyValue<string>(logEvent!, "Location");

        await Assert.That(eventLocationName)
            .IsEqualTo(locationName);
    }

    [Test]
    [Arguments("")]
    [Arguments("Testing")]
    [Arguments("Staging")]
    [Arguments("PreProduction")]
    public async Task InstallationEnvironmentName_ShouldBeApplied(string installationEnvName)
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithInstallationEnvironmentName(installationEnvName)
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        await Assert.That(logEvent)
            .IsNotNull();

        installationEnvName = installationEnvName != "" ? installationEnvName : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        var eventInstallationEnvName = GetPropertyValue<string>(logEvent!, "InstallationEnvironment");

        await Assert.That(eventInstallationEnvName)
            .IsEqualTo(installationEnvName);
    }

    [Test]
    public async Task OsInfo_ShouldBeApplied()
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithOsInfo()
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        await Assert.That(logEvent)
            .IsNotNull();

        var expectedOsInfo = RuntimeInformation.OSDescription;
        var osInfo = GetPropertyValue<string>(logEvent!, "OsInfo");

        await Assert.That(osInfo)
            .IsEqualTo(expectedOsInfo);
    }

    [Test]
    public async Task FrameworkVersion_ShouldBeApplied()
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithFrameworkVersion()
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        await Assert.That(logEvent)
            .IsNotNull();

        var expectedFrameworkVersion = RuntimeInformation.FrameworkDescription;
        var frameworkVersion = GetPropertyValue<string>(logEvent!, "FrameworkVersion");

        await Assert.That(frameworkVersion)
            .IsEqualTo(expectedFrameworkVersion);
    }
}
