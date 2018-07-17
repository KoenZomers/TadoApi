using System;
using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Converters
{
    /// <summary>
    /// Converts the duration mode type returned by the Tado API to the DurationModes enumerator in this project
    /// </summary>
    public class DurationModeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Enums.DurationModes durationMode = (Enums.DurationModes)value;

            switch (durationMode)
            {
                case Enums.DurationModes.UntilNextManualChange:
                    writer.WriteValue("MANUAL");
                    break;

                case Enums.DurationModes.UntilNextTimedEvent:
                    writer.WriteValue("TADO_MODE");
                    break;

                case Enums.DurationModes.Timer:
                    writer.WriteValue("TIMER");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = reader.Value.ToString();

            Enums.DurationModes? durationMode = null;
            switch(enumString)
            {
                case "MANUAL":
                    durationMode = Enums.DurationModes.UntilNextManualChange;
                    break;

                case "TADO_MODE":
                    durationMode = Enums.DurationModes.UntilNextTimedEvent;
                    break;

                case "TIMER":
                    durationMode = Enums.DurationModes.Timer;
                    break;
            }

            return durationMode;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}