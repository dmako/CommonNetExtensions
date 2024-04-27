using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Diagnostics;

namespace CommonNet.Json;

/// <summary>
/// Converts an Uri to or from JSON.
/// </summary>
public class AbsoluteUriJsonConverter : JsonConverter<Uri?>
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        Guard.IsNotNull(typeToConvert);

        return typeof(Uri).IsAssignableFrom(typeToConvert);
    }

    /// <inheritdoc />
    public override Uri? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!CanConvert(typeToConvert))
        {
            throw new JsonException("Not a valid Uri type.");
        }

        var value = reader.GetString();
        if (value is not null)
        {
            if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
            {
                return uri;
            }
            throw new JsonException("Not a valid absolute URI.");
        }

        return default;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Uri? value, JsonSerializerOptions options)
    {
        if (value is not null)
        {
            writer.WriteStringValue(value.OriginalString);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
