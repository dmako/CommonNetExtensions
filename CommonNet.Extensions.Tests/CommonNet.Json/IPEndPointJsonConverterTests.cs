#if NET6_0_OR_GREATER

using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace CommonNet.Json.Tests;

public class IPEndPointJsonConverterTests
{
    private readonly IPEndPointJsonConverter _converter = new IPEndPointJsonConverter();

    [Fact]
    public void CanConvert_Should_Return_True_For_IPEndPoint_Type()
    {
        var result = _converter.CanConvert(typeof(IPEndPoint));
        result.Should().BeTrue();
        result = _converter.CanConvert(typeof(EndPoint));
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("192.168.1.1:8080")]
    [InlineData("[::1]:8080")]
    public void Read_Should_Convert_Json_To_IPEndPoint(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPEndPoint), options);

        result.Should().NotBeNull();
        result!.ToString().Should().Be(jsonValue);
    }

    [Fact]
    public void Read_Should_Handle_Null_Value()
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes("null"));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPEndPoint), options);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("not_an_ip_endpoint")]
    [InlineData("")]
    public void Read_Should_Handle_Invalid_IPEndPoint(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        Type? exceptionType = null;
        try
        {
            reader.Read();
            var value = _converter.Read(ref reader, typeof(IPEndPoint), options);
        }
        catch (Exception ex)
        {
            exceptionType = ex.GetType();
        }
        exceptionType.Should().Be(typeof(JsonException));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("192.168.1.1:8080")]
    public void Write_Should_Convert_IPEndPoint_To_Json(string? endPointString)
    {
        using var ms = new MemoryStream();
        var writer = new Utf8JsonWriter(ms);
        var options = new JsonSerializerOptions();
        var endPoint = endPointString != null ? ParseIPEndPoint(endPointString) : null;

        _converter.Write(writer, endPoint, options);
        writer.Flush();

        var json = Encoding.UTF8.GetString(ms.ToArray());
        if (endPointString != null)
        {
            json.Should().Be($"\"{endPointString}\"");
        }
        else
        {
            json.Should().Be("null");
        }
    }

    private static IPEndPoint? ParseIPEndPoint(string endPointString)
    {
        try
        {
            var colonIndex = endPointString.LastIndexOf(':');
            var address = endPointString[..colonIndex];
            var port = int.Parse(endPointString[(colonIndex + 1)..]);
            IPAddress ip;
            if (address.StartsWith('[') && address.EndsWith(']'))
            {
                address = address[1..^1];
                ip = IPAddress.Parse(address);
            }
            else
            {
                ip = IPAddress.Parse(address);
            }
            return new IPEndPoint(ip, port);
        }
        catch
        {
            return null;
        }
    }
}

#endif
