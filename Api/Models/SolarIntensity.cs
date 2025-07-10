using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Represents the solar intensity measured by a Tado device
/// </summary>
public partial class SolarIntensity
{
    /// <summary>
    /// The type of solar intensity measurement
    /// </summary>
    [JsonPropertyName("type")]
    public string? CurrentType { get; set; }

    /// <summary>
    /// The percentage of solar intensity
    /// </summary>
    [JsonPropertyName("percentage")]
    public double? Percentage { get; set; }

    /// <summary>
    /// The timestamp when the solar intensity was recorded
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }
}
