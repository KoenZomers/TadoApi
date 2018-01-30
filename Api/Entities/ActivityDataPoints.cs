using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public class ActivityDataPoints
    {
        [JsonProperty("heatingPower")]
        public HeatingPower HeatingPower { get; set; }
    }
}