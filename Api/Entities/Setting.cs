using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Setting
    {
        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("power")]
        public string Power { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public Temperature Temperature { get; set; }
    }
}