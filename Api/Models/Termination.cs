using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Information about when the current state of the Tado device is expected to change
/// </summary>
public partial class Termination
{
    /// <summary>
    /// Defines if and what will make the Tado device change its state
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(Converters.DurationModeConverter))] // Ensure this is a System.Text.Json converter
    public Enums.DurationModes? CurrentType { get; set; }

    /// <summary>
    /// Date and time at which the termination mode is expected to change. NULL if CurrentType is Manual.
    /// </summary>
    [JsonPropertyName("projectedExpiry")]
    public DateTime? ProjectedExpiry { get; set; }

    /// <summary>
    /// Date and time at which the termination mode will change. NULL if CurrentType is Manual.
    /// </summary>
    [JsonPropertyName("expiry")]
    public DateTime? Expiry { get; set; }

    /// <summary>
    /// Amount of seconds remaining before the Tado device will change its state. Only set if CurrentType is Timer.
    /// </summary>
    [JsonPropertyName("durationInSeconds")]
    public int? DurationInSeconds { get; set; }
}
