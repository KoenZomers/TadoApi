using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Open Window Detection settings
    /// </summary>
    public partial class OpenWindowDetection
    {
        [JsonProperty("supported")]
        public bool Supported { get; set; }

        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty("timeoutInSeconds")]
        public long? TimeoutInSeconds { get; set; }
    }
}