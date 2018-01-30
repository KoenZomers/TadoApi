using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Details about the current configuration of the dazzle mode (animation when changing the temperature)
    /// </summary>
    public partial class DazzleMode
    {
        [JsonProperty("supported")]
        public bool Supported { get; set; }

        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
    }
}