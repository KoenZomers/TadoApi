using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Temperature and humidity measured by a Tado device
/// </summary>
public partial class SensorDataPoints
{
    /// <summary>
    /// The inside temperature data point
    /// </summary>
    [JsonPropertyName("insideTemperature")]
    public InsideTemperature? InsideTemperature { get; set; }

    /// <summary>
    /// The humidity data point
    /// </summary>
    [JsonPropertyName("humidity")]
    public Humidity? Humidity { get; set; }
}
