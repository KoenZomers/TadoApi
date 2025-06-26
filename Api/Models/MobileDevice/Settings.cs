using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models.MobileDevice;

/// <summary>
/// Contains settings specific to the device
/// </summary>
public class Settings
{
    /// <summary>
    /// Indicates whether geolocation tracking is enabled for the device
    /// </summary>
    [JsonPropertyName("geoTrackingEnabled")]
    public bool? GeoTrackingEnabled { get; set; }
}
