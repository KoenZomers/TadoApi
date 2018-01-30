using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Temperature and humidity measured by a Tado device
    /// </summary>
    public partial class SensorDataPoints
    {
        [JsonProperty("insideTemperature")]
        public InsideTemperature InsideTemperature { get; set; }

        [JsonProperty("humidity")]
        public Humidity Humidity { get; set; }
    }
}