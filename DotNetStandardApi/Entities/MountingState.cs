using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// State of the mounted Tado device
    /// </summary>
    public class MountingState
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTime Timestamp { get; set; }
    }
}