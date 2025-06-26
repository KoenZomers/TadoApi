using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Entities;

/// <summary>
/// Represents the link state of a Tado device or component
/// </summary>
public partial class Link
{
    /// <summary>
    /// The current state of the link (e.g., ONLINE, OFFLINE)
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }
}
