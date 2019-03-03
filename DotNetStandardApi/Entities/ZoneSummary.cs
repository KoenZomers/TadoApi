using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Summarized state of a zone
    /// </summary>
    public class ZoneSummary
    {
        /// <summary>
        /// The current state of the Tado device
        /// </summary>
        [JsonProperty("setting")]
        public Setting Setting { get; set; }

        /// <summary>
        /// Information on when the current state of the Tado device will end
        /// </summary>
        [JsonProperty("termination")]
        public Termination Termination { get; set; }
    }
}