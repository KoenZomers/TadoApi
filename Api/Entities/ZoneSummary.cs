using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Summarized state of a zone
    /// </summary>
    public class ZoneSummary
    {
        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("setting")]
        public Setting Setting { get; set; }

        [JsonProperty("termination")]
        public Termination Termination { get; set; }
    }
}