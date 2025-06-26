using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models.MobileDevice;

/// <summary>
/// Contains detailed information about a device connected to Tado
/// </summary>
public class Details
{
    /// <summary>
    /// The platform of the mobile device (e.g., iOS, Android)
    /// </summary>
    [JsonPropertyName("platform")]
    public string? Platform { get; set; }

    /// <summary>
    /// The operating system version of the mobile device
    /// </summary>
    [JsonPropertyName("osVersion")]
    public string? OsVersion { get; set; }

    /// <summary>
    /// The model of the mobile device
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    /// The locale setting of the mobile device
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }
}
