using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public class EarlyStart
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
