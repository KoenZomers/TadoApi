using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Represents the current overlay state of a Tado device
/// </summary>
public partial class Overlay
{
    /// <summary>
    /// The current setting applied to the Tado device
    /// </summary>
    [JsonPropertyName("setting")]
    public Setting? Setting { get; set; }

    /// <summary>
    /// Information on when the current setting will end
    /// </summary>
    [JsonPropertyName("termination")]
    public Termination? Termination { get; set; }
}
