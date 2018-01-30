using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Temperatures
    {
        [JsonProperty("celsius")]
        public TemperatureSteps Celsius { get; set; }

        [JsonProperty("fahrenheit")]
        public TemperatureSteps Fahrenheit { get; set; }
    }
}