using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Information about one zone
/// </summary>
public class Zone
{
    /// <summary>
    /// The unique identifier of the zone
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    /// <summary>
    /// The name of the zone
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The current type of the zone (e.g., HEATING, HOT_WATER)
    /// </summary>
    [JsonPropertyName("type")]
    public string? CurrentType { get; set; }

    /// <summary>
    /// The date and time when the zone was created
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// The types of devices associated with the zone
    /// </summary>
    [JsonPropertyName("deviceTypes")]
    public string[]? DeviceTypes { get; set; }

    /// <summary>
    /// The list of devices in the zone
    /// </summary>
    [JsonPropertyName("devices")]
    public Device[]? Devices { get; set; }

    /// <summary>
    /// Indicates whether a report is available for the zone
    /// </summary>
    [JsonPropertyName("reportAvailable")]
    public bool? ReportAvailable { get; set; }

    /// <summary>
    /// Indicates whether the zone supports the Dazzle feature
    /// </summary>
    [JsonPropertyName("supportsDazzle")]
    public bool? SupportsDazzle { get; set; }

    /// <summary>
    /// Indicates whether the Dazzle feature is enabled
    /// </summary>
    [JsonPropertyName("dazzleEnabled")]
    public bool? DazzleEnabled { get; set; }

    /// <summary>
    /// The current Dazzle mode configuration
    /// </summary>
    [JsonPropertyName("dazzleMode")]
    public DazzleMode? DazzleMode { get; set; }

    /// <summary>
    /// The open window detection settings for the zone
    /// </summary>
    [JsonPropertyName("openWindowDetection")]
    public OpenWindowDetection? OpenWindowDetection { get; set; }
}
