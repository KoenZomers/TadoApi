using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Converters
{
    /// <summary>
    /// Converts the duration mode type returned by the Tado API to the DurationModes enumerator in this project
    /// </summary>
    public class DurationModeConverter : JsonConverter<Enums.DurationModes?>
    {
        public override Enums.DurationModes? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                "MANUAL" => Enums.DurationModes.UntilNextManualChange,
                "TADO_MODE" => Enums.DurationModes.UntilNextTimedEvent,
                "TIMER" => Enums.DurationModes.Timer,
                _ => null
            };
        }

        public override void Write(Utf8JsonWriter writer, Enums.DurationModes? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            switch (value)
            {
                case Enums.DurationModes.UntilNextManualChange:
                    writer.WriteStringValue("MANUAL");
                    break;

                case Enums.DurationModes.UntilNextTimedEvent:
                    writer.WriteStringValue("TADO_MODE");
                    break;

                case Enums.DurationModes.Timer:
                    writer.WriteStringValue("TIMER");
                    break;

                default:
                    writer.WriteNullValue();
                    break;
            }
        }
    }
}