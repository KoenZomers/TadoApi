using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities.MobileDevice
{
    /// <summary>
    /// Contains the location of a device
    /// </summary>
    public class Location
    {
        [JsonProperty("stale")]
        public bool Stale { get; set; }

        [JsonProperty("atHome")]
        public bool AtHome { get; set; }

        [JsonProperty("bearingFromHome")]
        public BearingFromHome BearingFromHome { get; set; }

        [JsonProperty("relativeDistanceFromHomeFence")]
        public long RelativeDistanceFromHomeFence { get; set; }
    }
}