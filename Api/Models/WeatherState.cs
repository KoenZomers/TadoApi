using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Entities;

/// <summary>
/// Represents the current weather state
/// </summary>
public partial class WeatherState
{
    /// <summary>
    /// The type of weather state (e.g., SUNNY, CLOUDY)
    /// </summary>
    [JsonPropertyName("type")]
    public string? CurrentType { get; set; }

    /// <summary>
    /// The value describing the weather condition
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// The timestamp when the weather state was recorded
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }
}
