using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Temperature
    {
        [JsonProperty("celsius", NullValueHandling = NullValueHandling.Ignore)]
        public double? Celsius { get; set; }

        [JsonProperty("fahrenheit", NullValueHandling = NullValueHandling.Ignore)]
        public double? Fahrenheit { get; set; }
    }
}