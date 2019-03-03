using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public class TemperatureSteps
    {
        [JsonProperty("min")]
        public long Min { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }

        [JsonProperty("step")]
        public long Step { get; set; }
    }
}