using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Represents the minimum, maximum, and step values for temperature settings
/// </summary>
public class TemperatureSteps
{
    /// <summary>
    /// The minimum temperature value
    /// </summary>
    [JsonPropertyName("min")]
    public long? Min { get; set; }

    /// <summary>
    /// The maximum temperature value
    /// </summary>
    [JsonPropertyName("max")]
    public long? Max { get; set; }

    /// <summary>
    /// The step increment for temperature adjustments
    /// </summary>
    [JsonPropertyName("step")]
    public long? Step { get; set; }
}
