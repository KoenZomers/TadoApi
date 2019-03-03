using System;
using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Converters
{
    /// <summary>
    /// Converts the power state returned by the Tado API to the PowerStates enumerator in this project
    /// </summary>
    public class PowerStatesConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Enums.PowerStates powerState = (Enums.PowerStates)value;

            switch (powerState)
            {
                case Enums.PowerStates.On:
                    writer.WriteValue("ON");
                    break;

                case Enums.PowerStates.Off:
                    writer.WriteValue("OFF");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = reader.Value.ToString();

            Enums.PowerStates? powerState = null;
            switch(enumString)
            {
                case "ON":
                    powerState = Enums.PowerStates.On;
                    break;

                case "OFF":
                    powerState = Enums.PowerStates.Off;
                    break;
            }

            return powerState;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}