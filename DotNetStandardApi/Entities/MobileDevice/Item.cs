using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities.MobileDevice
{
    /// <summary>
    /// Contains information about a mobile device set up to be used with Tado
    /// </summary>
    public class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("deviceMetadata")]
        public Details MobileDeviceDetails { get; set; }
    }
}