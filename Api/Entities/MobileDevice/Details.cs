using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities.MobileDevice
{
    /// <summary>
    /// Contains detailed information about a device connected to Tado
    /// </summary>
    public class Details
    {
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("osVersion")]
        public string OsVersion { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
    }
}