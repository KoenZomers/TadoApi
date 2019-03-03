using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// State of a specific zone
    /// </summary>
    public class State
    {
        [JsonProperty("tadoMode")]
        public string TadoMode { get; set; }

        [JsonProperty("geolocationOverride")]
        public bool GeolocationOverride { get; set; }

        [JsonProperty("geolocationOverrideDisableTime")]
        public object GeolocationOverrideDisableTime { get; set; }

        [JsonProperty("preparation")]
        public object Preparation { get; set; }

        [JsonProperty("setting")]
        public Setting Setting { get; set; }

        [JsonProperty("overlayType")]
        public string OverlayType { get; set; }

        [JsonProperty("overlay")]
        public Overlay Overlay { get; set; }

        [JsonProperty("openWindow")]
        public object OpenWindow { get; set; }

        [JsonProperty("link")]
        public Link Link { get; set; }

        [JsonProperty("activityDataPoints")]
        public ActivityDataPoints ActivityDataPoints { get; set; }

        [JsonProperty("sensorDataPoints")]
        public SensorDataPoints SensorDataPoints { get; set; }
    }
}
