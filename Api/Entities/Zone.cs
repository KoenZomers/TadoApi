using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Information about one zone
    /// </summary>
    public class Zone
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("dateCreated")]
        public System.DateTime DateCreated { get; set; }

        [JsonProperty("deviceTypes")]
        public string[] DeviceTypes { get; set; }

        [JsonProperty("devices")]
        public Device[] Devices { get; set; }

        [JsonProperty("reportAvailable")]
        public bool ReportAvailable { get; set; }

        [JsonProperty("supportsDazzle")]
        public bool SupportsDazzle { get; set; }

        [JsonProperty("dazzleEnabled")]
        public bool DazzleEnabled { get; set; }

        [JsonProperty("dazzleMode")]
        public DazzleMode DazzleMode { get; set; }

        [JsonProperty("openWindowDetection")]
        public OpenWindowDetection OpenWindowDetection { get; set; }
    }
}