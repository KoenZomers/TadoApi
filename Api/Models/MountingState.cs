using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Entities;

/// <summary>
/// State of the mounted Tado device
/// </summary>
public class MountingState
{
    /// <summary>
    /// The current mounting state of the device (e.g., MOUNTED, UNMOUNTED)
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// The timestamp when the mounting state was recorded
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }
}
