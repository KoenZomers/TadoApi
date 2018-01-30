using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public class Geolocation
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}