using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Information regarding a temperature
    /// </summary>
    public partial class Temperature
    {
        /// <summary>
        /// The temperature in degrees Celcius
        /// </summary>
        [JsonProperty("celsius", NullValueHandling = NullValueHandling.Ignore)]
        public double? Celsius { get; set; }

        /// <summary>
        /// The temperature in degrees Fahrenheit
        /// </summary>
        [JsonProperty("fahrenheit", NullValueHandling = NullValueHandling.Ignore)]
        public double? Fahrenheit { get; set; }
    }
}