using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Information about one Tado device
    /// </summary>
    public class Device
    {
        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty("serialNo")]
        public string SerialNo { get; set; }

        [JsonProperty("shortSerialNo")]
        public string ShortSerialNo { get; set; }

        [JsonProperty("currentFwVersion")]
        public string CurrentFwVersion { get; set; }

        [JsonProperty("connectionState")]
        public ConnectionState ConnectionState { get; set; }

        [JsonProperty("characteristics")]
        public Characteristics Characteristics { get; set; }

        [JsonProperty("duties")]
        public string[] Duties { get; set; }

        [JsonProperty("mountingState")]
        public MountingState MountingState { get; set; }

        [JsonProperty("batteryState")]
        public string BatteryState { get; set; }
    }
}