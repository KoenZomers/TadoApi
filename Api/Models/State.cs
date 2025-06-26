using System.Text.Json.Serialization;
using KoenZomers.Tado.Api.Entities;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// State of a specific zone
/// </summary>
public class State
{
    /// <summary>
    /// The current Tado mode (e.g., HOME, AWAY)
    /// </summary>
    [JsonPropertyName("tadoMode")]
    public string? TadoMode { get; set; }

    /// <summary>
    /// Indicates whether geolocation override is active
    /// </summary>
    [JsonPropertyName("geolocationOverride")]
    public bool? GeolocationOverride { get; set; }

    /// <summary>
    /// The time when geolocation override will be disabled
    /// </summary>
    [JsonPropertyName("geolocationOverrideDisableTime")]
    public object? GeolocationOverrideDisableTime { get; set; }

    /// <summary>
    /// Preparation state information (if any)
    /// </summary>
    [JsonPropertyName("preparation")]
    public object? Preparation { get; set; }

    /// <summary>
    /// The current setting applied to the zone
    /// </summary>
    [JsonPropertyName("setting")]
    public Setting? Setting { get; set; }

    /// <summary>
    /// The type of overlay currently active
    /// </summary>
    [JsonPropertyName("overlayType")]
    public string? OverlayType { get; set; }

    /// <summary>
    /// The overlay configuration currently applied
    /// </summary>
    [JsonPropertyName("overlay")]
    public Overlay? Overlay { get; set; }

    /// <summary>
    /// Information about an open window event (if any)
    /// </summary>
    [JsonPropertyName("openWindow")]
    public object? OpenWindow { get; set; }

    /// <summary>
    /// Indicates whether an open window has been detected
    /// </summary>
    [JsonPropertyName("openWindowDetected")]
    public bool? OpenWindowDetected { get; set; }

    /// <summary>
    /// The link state of the zone
    /// </summary>
    [JsonPropertyName("link")]
    public Link? Link { get; set; }

    /// <summary>
    /// Activity data points such as heating power
    /// </summary>
    [JsonPropertyName("activityDataPoints")]
    public ActivityDataPoints? ActivityDataPoints { get; set; }

    /// <summary>
    /// Sensor data points such as temperature and humidity
    /// </summary>
    [JsonPropertyName("sensorDataPoints")]
    public SensorDataPoints? SensorDataPoints { get; set; }
}
