using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Overlay
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentType { get; set; }

        [JsonProperty("setting")]
        public Setting Setting { get; set; }

        [JsonProperty("termination")]
        public Termination Termination { get; set; }
    }
}