using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public class HeatingPower
    {
        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("percentage")]
        public long Percentage { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTime Timestamp { get; set; }
    }
}