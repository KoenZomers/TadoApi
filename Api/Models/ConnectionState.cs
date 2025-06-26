using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models
{
    /// <summary>
    /// State of the connection towards a Tado device
    /// </summary>
    public partial class ConnectionState
    {
        [JsonPropertyName("value")]
        public bool Value { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}