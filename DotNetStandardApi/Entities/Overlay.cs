using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Overlay
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