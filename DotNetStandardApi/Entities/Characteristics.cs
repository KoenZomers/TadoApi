using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Characteristics of a device
    /// </summary>
    public class Characteristics
    {
        [JsonProperty("capabilities")]
        public string[] Capabilities { get; set; }
    }
}