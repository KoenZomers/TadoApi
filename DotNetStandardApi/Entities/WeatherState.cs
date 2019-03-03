using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class WeatherState
    {
        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTime Timestamp { get; set; }
    }
}