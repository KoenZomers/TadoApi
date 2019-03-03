using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities.MobileDevice
{
    /// <summary>
    /// Contains settings specific to the device
    /// </summary>
    public class Settings
    {
        [JsonProperty("geoTrackingEnabled")]
        public bool GeoTrackingEnabled { get; set; }
    }
}