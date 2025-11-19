using System.Text.Json;
using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Converters;

/// <summary>
/// Converts the power state returned by the Tado API to the PowerStates enumerator in this project
/// </summary>
public class PowerStatesConverter : JsonConverter<Enums.PowerStates?>
{
    public override Enums.PowerStates? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            "ON" => Enums.PowerStates.On,
            "OFF" => Enums.PowerStates.Off,
            _ => null
        };
    }

    public override void Write(Utf8JsonWriter writer, Enums.PowerStates? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        switch (value)
        {
            case Enums.PowerStates.On:
                writer.WriteStringValue("ON");
                break;

            case Enums.PowerStates.Off:
                writer.WriteStringValue("OFF");
                break;

            default:
                writer.WriteNullValue();
                break;
        }
    }
}