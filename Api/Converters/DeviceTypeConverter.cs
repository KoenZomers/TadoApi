using System.Text.Json;
using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Converters;

/// <summary>
/// Converts the Tado device type returned by the Tado API to the DeviceTypes enumerator in this project
/// </summary>
public class DeviceTypeConverter : JsonConverter<Enums.DeviceTypes?>
{
    public override Enums.DeviceTypes? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            return null;
        }

        var enumString = reader.GetString();
        if (string.IsNullOrEmpty(enumString))
        {
            return null;
        }

        return enumString switch
        {
            "HEATING" => Enums.DeviceTypes.Heating,
            "HOT_WATER" => Enums.DeviceTypes.HotWater,
            _ => null
        };
    }

    public override void Write(Utf8JsonWriter writer, Enums.DeviceTypes? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        switch (value)
        {
            case Enums.DeviceTypes.Heating:
                writer.WriteStringValue("HEATING");
                break;

            case Enums.DeviceTypes.HotWater:
                writer.WriteStringValue("HOT_WATER");
                break;

            default:
                writer.WriteNullValue();
                break;
        }
    }
}