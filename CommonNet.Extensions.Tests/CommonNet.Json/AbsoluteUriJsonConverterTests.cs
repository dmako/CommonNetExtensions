using System.Text;
using System.Text.Json;

namespace CommonNet.Json.Tests;

public class AbsoluteUriJsonConverterTests
{
    private readonly AbsoluteUriJsonConverter _converter = new();

    [Test]
    public async Task CanConvert_Should_Return_True_For_Uri_Type()
    {
        var result = _converter.CanConvert(typeof(Uri));
        await Assert.That(result)
            .IsTrue();
        result = _converter.CanConvert(typeof(UriBuilder));
        await Assert.That(result)
            .IsFalse();
    }

    [Test]
    [Arguments("https://example.com")]
    [Arguments("file:///path/to/resource")]
    [Arguments("https://example.com/?someparam=somevalue")]
    public async Task Read_Should_Convert_Json_To_Uri(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(Uri), options);

        await Assert.That(result)
            .IsNotNull();
        await Assert.That(result!.OriginalString)
            .IsEqualTo(jsonValue);
    }

    [Test]
    public async Task Read_Should_Handle_Null_Value()
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes("null"));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(Uri), options);

        await Assert.That(result)
            .IsNull();
    }

    [Test]
    [Arguments("not_a_valid_uri:")]
    [Arguments("     ")]
    public async Task Read_Should_Handle_Invalid_Uri(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        Type? exceptionType = null;
        try
        {
            reader.Read();
            var value = _converter.Read(ref reader, typeof(Uri), options);
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
    [Arguments("https://example.com")]
    public async Task Write_Should_Convert_Uri_To_Json(string? uriString)
    {
        using var ms = new MemoryStream();
        var writer = new Utf8JsonWriter(ms);
        var options = new JsonSerializerOptions();
        var uri = uriString != null ? new Uri(uriString) : null;

        _converter.Write(writer, uri, options);
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(ms.ToArray());
        if (uriString is not null)
        {
            await Assert.That(json)
                .IsEqualTo($"\"{uriString}\"");
        }
        else
        {
            await Assert.That(json)
                .IsEqualTo("null");
        }
    }
}
