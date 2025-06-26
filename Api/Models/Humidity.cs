using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Humidity measured by a Tado device
/// </summary>
public partial class Humidity
{
    /// <summary>
    /// The type of humidity measurement (e.g., PERCENTAGE)
    /// </summary>
    [JsonPropertyName("type")]
    public string? CurrentType { get; set; }

    /// <summary>
    /// The humidity percentage
    /// </summary>
    [JsonPropertyName("percentage")]
    public double? Percentage { get; set; }

    /// <summary>
    /// The timestamp when the humidity was measured
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }
}
