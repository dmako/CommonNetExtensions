using System.Net;
using System.Text;
using System.Text.Json;

namespace CommonNet.Json.Tests;

public class IPAddressJsonConverterTests
{
    private readonly IPAddressJsonConverter _converter = new IPAddressJsonConverter();

    [Test]
    public async Task CanConvert_Should_Return_True_For_IPAddress_Type()
    {
        var result = _converter.CanConvert(typeof(IPAddress));
        await Assert.That(result)
            .IsTrue();
        result = _converter.CanConvert(typeof(EndPoint));
        await Assert.That(result)
            .IsFalse();
    }

    [Test]
    [Arguments("192.168.1.1")]
    [Arguments("::1")]
    public async Task Read_Should_Convert_Json_To_IPAddress(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPAddress), options);

        await Assert.That(result)
            .IsNotNull()
            .And.IsEqualTo(IPAddress.Parse(jsonValue));
    }

    [Test]
    public async Task Read_Should_Handle_Null_Value()
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes("null"));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(IPAddress), options);

        await Assert.That(result)
            .IsNull();
    }

    [Test]
    [Arguments("not_an_ip_address")]
    [Arguments("")]
    public async Task Read_Should_Handle_Invalid_IPAddress(string jsonValue)
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
        await Assert.That(exceptionType)
            .IsEqualTo(typeof(JsonException));
    }

    [Test]
    [Arguments(null)]
    [Arguments("192.168.1.1")]
    public async Task Write_Should_Convert_IPAddress_To_Json(string? ipAddressString)
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
            await Assert.That(json)
                .IsEqualTo($"\"{ipAddressString}\"");
        }
        else
        {
            await Assert.That(json)
                .IsEqualTo("null");
        }
    }
}
