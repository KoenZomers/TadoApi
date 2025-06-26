using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Entities;

/// <summary>
/// Represents the precision of a temperature reading
/// </summary>
public partial class Precision
{
    /// <summary>
    /// The precision in Celsius
    /// </summary>
    [JsonPropertyName("celsius")]
    public long? Celsius { get; set; }

    /// <summary>
    /// The precision in Fahrenheit
    /// </summary>
    [JsonPropertyName("fahrenheit")]
    public long? Fahrenheit { get; set; }
}
