using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// State of the connection towards a Tado device
    /// </summary>
    public partial class ConnectionState
    {
        [JsonProperty("value")]
        public bool Value { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTime Timestamp { get; set; }
    }
}