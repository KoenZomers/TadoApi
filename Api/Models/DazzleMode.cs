using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Details about the current configuration of the dazzle mode (animation when changing the temperature)
/// </summary>
public partial class DazzleMode
{
    /// <summary>
    /// Indicates whether the dazzle mode feature is supported
    /// </summary>
    [JsonPropertyName("supported")]
    public bool? Supported { get; set; }

    /// <summary>
    /// Indicates whether the dazzle mode feature is currently enabled
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
}
