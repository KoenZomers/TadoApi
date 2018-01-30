using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Humidity measured by a Tado device
    /// </summary>
    public partial class Humidity
    {
        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("percentage")]
        public double Percentage { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTime Timestamp { get; set; }
    }
}