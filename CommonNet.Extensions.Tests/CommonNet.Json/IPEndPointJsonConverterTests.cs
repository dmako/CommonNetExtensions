#if NET6_0_OR_GREATER

using System.Net;
using System.Text;
using System.Text.Json;

namespace CommonNet.Json.Tests;

public class IPEndPointJsonConverterTests
{
    private readonly IPEndPointJsonConverter _converter = new();

    [Test]
    public async Task CanConvert_Should_Return_True_For_IPEndPoint_Type()
    {
        var result = _converter.CanConvert(typeof(IPEndPoint));
        await Assert.That(result)
            .IsTrue();
        result = _converter.CanConvert(typeof(EndPoint));
        await Assert.That(result)
            .IsFalse();
    }

    [Test]
    [Arguments("192.168.1.1:8080")]
    [Arguments("[::1]:8080")]
    public async Task Read_Should_Convert_Json_To_IPEndPoint(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPEndPoint), options);

        await Assert.That(result)
            .IsNotNull();
        await Assert.That(result!.ToString())
            .IsEqualTo(jsonValue);
    }

    [Test]
    public async Task Read_Should_Handle_Null_Value()
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes("null"));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPEndPoint), options);

        await Assert.That(result)
            .IsNull();
    }

    [Test]
    [Arguments("not_an_ip_endpoint")]
    [Arguments("")]
    public async Task Read_Should_Handle_Invalid_IPEndPoint(string jsonValue)
    {
        await Assert.That(() =>
        {
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
            var options = new JsonSerializerOptions();
            reader.Read();
            _ = _converter.Read(ref reader, typeof(IPEndPoint), options);
        }).Throws<JsonException>();
    }

    [Test]
    [Arguments(null)]
    [Arguments("192.168.1.1:8080")]
    public async Task Write_Should_Convert_IPEndPoint_To_Json(string? endPointString)
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
            await Assert.That(json)
                .IsEqualTo($"\"{endPointString}\"");
        }
        else
        {
            await Assert.That(json)
                .IsEqualTo("null");
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
