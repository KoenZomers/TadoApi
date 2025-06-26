using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models.MobileDevice;

/// <summary>
/// Contains information about a mobile device set up to be used with Tado
/// </summary>
public class Item
{
    /// <summary>
    /// The name of the mobile device
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The unique identifier of the mobile device
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    /// <summary>
    /// The settings configured for the mobile device
    /// </summary>
    [JsonPropertyName("settings")]
    public Settings? Settings { get; set; }

    /// <summary>
    /// The location information of the mobile device
    /// </summary>
    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    /// <summary>
    /// Metadata details about the mobile device
    /// </summary>
    [JsonPropertyName("deviceMetadata")]
    public Details? MobileDeviceDetails { get; set; }
}
