using System;
using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Converters
{
    /// <summary>
    /// Converts the Tado device type returned by the Tado API to the DeviceTypes enumerator in this project
    /// </summary>
    public class DeviceTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Enums.DeviceTypes terminationType = (Enums.DeviceTypes)value;

            switch (terminationType)
            {
                case Enums.DeviceTypes.Heating:
                    writer.WriteValue("HEATING");
                    break;

                case Enums.DeviceTypes.HotWater:
                    writer.WriteValue("HOT_WATER");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = reader.Value.ToString();

            Enums.DeviceTypes? terminationType = null;
            switch(enumString)
            {
                case "HEATING":
                    terminationType = Enums.DeviceTypes.Heating;
                    break;

                case "HOT_WATER":
                    terminationType = Enums.DeviceTypes.HotWater;
                    break;
            }

            return terminationType;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
