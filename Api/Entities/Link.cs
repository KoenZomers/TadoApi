using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Link
    {
        [JsonProperty("state")]
        public string State { get; set; }
    }
}