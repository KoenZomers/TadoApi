using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Information about the state of the home
    /// </summary>
    public class HomeState
    {
        [JsonProperty("presence")]
        public string Presence { get; set; }
    }
}