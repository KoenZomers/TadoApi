using System.Text.Json.Serialization;
using KoenZomers.Tado.Api.Entities;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// The current weather
/// </summary>
public partial class Weather
{
    /// <summary>
    /// The current solar intensity
    /// </summary>
    [JsonPropertyName("solarIntensity")]
    public SolarIntensity? SolarIntensity { get; set; }

    /// <summary>
    /// The current outside temperature
    /// </summary>
    [JsonPropertyName("outsideTemperature")]
    public OutsideTemperature? OutsideTemperature { get; set; }

    /// <summary>
    /// The current weather state (e.g., SUNNY, CLOUDY)
    /// </summary>
    [JsonPropertyName("weatherState")]
    public WeatherState? WeatherState { get; set; }
}
