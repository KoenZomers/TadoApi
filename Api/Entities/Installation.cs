using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// One Tado installation
    /// </summary>
    public class Installation
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("revision")]
        public long Revision { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("devices")]
        public Device[] Devices { get; set; }
    }
}
