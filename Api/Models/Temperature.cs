using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Information regarding a temperature
/// </summary>
public partial class Temperature
{
    /// <summary>
    /// The temperature in degrees Celsius
    /// </summary>
    [JsonPropertyName("celsius")]
    public double? Celsius { get; set; }

    /// <summary>
    /// The temperature in degrees Fahrenheit
    /// </summary>
    [JsonPropertyName("fahrenheit")]
    public double? Fahrenheit { get; set; }
}
