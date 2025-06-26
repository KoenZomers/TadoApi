using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Models
{
    /// <summary>
    /// State of the connection towards a Tado device
    /// </summary>
    public partial class ConnectionState
    {
        [JsonProperty("value")]
        public bool Value { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}