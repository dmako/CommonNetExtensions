using System.Runtime.InteropServices;
using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace CommonNet.Logging.Tests;

public class EnrichersTests
{

    private static TResult GetPropertyValue<TResult>(LogEvent logEvent, string propertyName)
    {
        return (TResult)(((ScalarValue)logEvent.Properties[propertyName]!).Value!);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Switzerland")]
    [InlineData("Czechia")]
    [InlineData("Prague")]
    public void LocationEnricher_ShouldBeApplied(string locationName)
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithLocation(locationName)
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        logEvent.Should().NotBeNull();

        locationName = locationName != "" ? locationName : Environment.MachineName;
        var eventLocationName = GetPropertyValue<string>(logEvent!, "Location");

        eventLocationName.Should().Be(locationName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Testing")]
    [InlineData("Staging")]
    [InlineData("PreProduction")]
    public void InstallationEnvironmentName_ShouldBeApplied(string installationEnvName)
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithInstallationEnvironmentName(installationEnvName)
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        logEvent.Should().NotBeNull();

        installationEnvName = installationEnvName != "" ? installationEnvName : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        var eventInstallationEnvName = GetPropertyValue<string>(logEvent!, "InstallationEnvironment");

        eventInstallationEnvName.Should().Be(installationEnvName);
    }

    [Fact]
    public void OsInfo_ShouldBeApplied()
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithOsInfo()
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        logEvent.Should().NotBeNull();

        var expectedOsInfo = RuntimeInformation.OSDescription;
        var osInfo = GetPropertyValue<string>(logEvent!, "OsInfo");

        osInfo.Should().Be(expectedOsInfo);
    }

    [Fact]
    public void FrameworkVersion_ShouldBeApplied()
    {
        LogEvent? logEvent = null;
        var log = new LoggerConfiguration()
            .Enrich.WithFrameworkVersion()
            .WriteTo.Sink(new TestSink(e => logEvent = e))
            .CreateLogger();

        log.Information("Test Message With Properties");

        logEvent.Should().NotBeNull();

        var expectedFrameworkVersion = RuntimeInformation.FrameworkDescription;
        var frameworkVersion = GetPropertyValue<string>(logEvent!, "FrameworkVersion");

        frameworkVersion.Should().Be(expectedFrameworkVersion);
    }
}
