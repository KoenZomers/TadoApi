using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// The current weather
    /// </summary>
    public partial class Weather
    {
        [JsonProperty("solarIntensity")]
        public SolarIntensity SolarIntensity { get; set; }

        [JsonProperty("outsideTemperature")]
        public OutsideTemperature OutsideTemperature { get; set; }

        [JsonProperty("weatherState")]
        public WeatherState WeatherState { get; set; }
    }
}