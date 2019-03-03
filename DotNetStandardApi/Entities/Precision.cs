using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Precision
    {
        [JsonProperty("celsius")]
        public long Celsius { get; set; }

        [JsonProperty("fahrenheit")]
        public long Fahrenheit { get; set; }
    }

}