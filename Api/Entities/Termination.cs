using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Termination
    {
        [JsonProperty("type")]
        public string CurrentType { get; set; }

        [JsonProperty("projectedExpiry", NullValueHandling = NullValueHandling.Ignore)]
        public object ProjectedExpiry { get; set; }

        [JsonProperty("durationInSeconds", NullValueHandling = NullValueHandling.Ignore)]
        public int DurationInSeconds { get; set; }
    }
}