using KoenZomers.Tado.Api.Models;
using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Entities;

/// <summary>
/// Represents temperature step settings in both Celsius and Fahrenheit
/// </summary>
public partial class Temperatures
{
    /// <summary>
    /// Temperature step settings in Celsius
    /// </summary>
    [JsonPropertyName("celsius")]
    public TemperatureSteps? Celsius { get; set; }

    /// <summary>
    /// Temperature step settings in Fahrenheit
    /// </summary>
    [JsonPropertyName("fahrenheit")]
    public TemperatureSteps? Fahrenheit { get; set; }
}
