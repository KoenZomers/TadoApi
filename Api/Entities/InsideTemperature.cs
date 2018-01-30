using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class InsideTemperature
    {
        [JsonProperty("celsius")]
        public double Celsius { get; set; }

        [JsonProperty("fahrenheit")]
        public double Fahrenheit { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTime Timestamp { get; set; }

        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("precision")]
        public Precision Precision { get; set; }
    }
}