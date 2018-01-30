using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public class Capability
    {
        [JsonProperty("type")]
        public string PurpleType { get; set; }

        [JsonProperty("temperatures")]
        public Temperatures Temperatures { get; set; }
    }
}