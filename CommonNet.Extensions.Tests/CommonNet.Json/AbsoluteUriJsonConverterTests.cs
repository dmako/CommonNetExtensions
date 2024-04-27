using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace CommonNet.Json.Tests;

public class AbsoluteUriJsonConverterTests
{
    private readonly AbsoluteUriJsonConverter _converter = new();

    [Fact]
    public void CanConvert_Should_Return_True_For_Uri_Type()
    {
        var result = _converter.CanConvert(typeof(Uri));
        result.Should().BeTrue();
        result = _converter.CanConvert(typeof(UriBuilder));
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("file:///path/to/resource")]
    [InlineData("https://example.com/?someparam=somevalue")]
    public void Read_Should_Convert_Json_To_Uri(string jsonValue)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{jsonValue}\""));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(Uri), options);

        result.Should().NotBeNull();
        result!.OriginalString.Should().Be(jsonValue);
    }

    [Fact]
    public void Read_Should_Handle_Null_Value()
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes("null"));
        var options = new JsonSerializerOptions();

        reader.Read();
        var result = _converter.Read(ref reader, typeof(Uri), options);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("not_a_valid_uri:")]
    [InlineData("     ")]
    public void Read_Should_Handle_Invalid_Uri(string jsonValue)
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
        exceptionType.Should().Be(typeof(JsonException));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("https://example.com")]
    public void Write_Should_Convert_Uri_To_Json(string? uriString)
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
            json.Should().Be($"\"{uriString}\"");
        }
        else
        {
            json.Should().Be("null");
        }
    }
}
