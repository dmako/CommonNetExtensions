using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Diagnostics;

namespace CommonNet.Json;

/// <summary>
/// Converts an IPAddress to or from JSON.
/// </summary>
public class IPAddressJsonConverter : JsonConverter<IPAddress?>
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        Guard.IsNotNull(typeToConvert);

        return typeof(IPAddress).IsAssignableFrom(typeToConvert);
    }

    /// <inheritdoc />
    public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!CanConvert(typeToConvert))
        {
            throw new JsonException("Not a valid IPAddress type.");
        }

        var value = reader.GetString();
        if (value is not null)
        {
            if (IPAddress.TryParse(value, out var ip))
            {
                return ip;
            }
            throw new JsonException("Not a valid IP address.");
        }

        return default;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IPAddress? value, JsonSerializerOptions options)
    {
        if (value is not null)
        {
            writer.WriteStringValue(value.ToString());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
