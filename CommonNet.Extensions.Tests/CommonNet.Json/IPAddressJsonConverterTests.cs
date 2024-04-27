using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace CommonNet.Json.Tests;

public class IPAddressJsonConverterTests
{
    private readonly IPAddressJsonConverter _converter = new IPAddressJsonConverter();

    [Fact]
    public void CanConvert_Should_Return_True_For_IPAddress_Type()
    {
        var result = _converter.CanConvert(typeof(IPAddress));
        result.Should().BeTrue();
        result = _converter.CanConvert(typeof(EndPoint));
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("192.168.1.1")]
    [InlineData("::1")]
    public void Read_Should_Convert_Json_To_IPAddress(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPAddress), options);

        result.Should().NotBeNull();
        result!.ToString().Should().Be(jsonValue);
    }

    [Fact]
    public void Read_Should_Handle_Null_Value()
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes("null"));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPAddress), options);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("not_an_ip_address")]
    [InlineData("")]
    public void Read_Should_Handle_Invalid_IPAddress(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        Type? exceptionType = null;
        try
        {
            reader.Read();
            var value = _converter.Read(ref reader, typeof(IPAddress), options);
        }
        catch (Exception ex)
        {
            exceptionType = ex.GetType();
        }
        exceptionType.Should().Be(typeof(JsonException));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("192.168.1.1")]
    public void Write_Should_Convert_IPAddress_To_Json(string? ipAddressString)
    {
        using var ms = new MemoryStream();
        var writer = new Utf8JsonWriter(ms);
        var options = new JsonSerializerOptions();
        var ipAddress = ipAddressString != null ? IPAddress.Parse(ipAddressString) : null;

        _converter.Write(writer, ipAddress, options);
        writer.Flush();

        var json = Encoding.UTF8.GetString(ms.ToArray());
        if (ipAddressString != null)
        {
            json.Should().Be($"\"{ipAddressString}\"");
        }
        else
        {
            json.Should().Be("null");
        }
    }
}
