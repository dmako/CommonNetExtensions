#if NET6_0_OR_GREATER

using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Diagnostics;

namespace CommonNet.Json;

/// <summary>
/// Converts an IPEndpoint to or from JSON.
/// </summary>
public class IPEndPointJsonConverter : JsonConverter<IPEndPoint?>
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        Guard.IsNotNull(typeToConvert);

        return typeof(IPEndPoint).IsAssignableFrom(typeToConvert);
    }

    /// <inheritdoc />
    public override IPEndPoint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!CanConvert(typeToConvert))
        {
            throw new JsonException("Not a valid IPEndPoint type.");
        }

        var value = reader.GetString();
        if (value is not null)
        {
            if (IPEndPoint.TryParse(value, out var endpoint))
            {
                return endpoint;
            }
            throw new JsonException("Not a valid IPEndPoint.");
        }

        return default;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IPEndPoint? value, JsonSerializerOptions options)
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

#endif
