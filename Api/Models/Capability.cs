using KoenZomers.Tado.Api.Entities;
using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Represents a capability of a Tado device, such as temperature control
/// </summary>
public class Capability
{
    /// <summary>
    /// The type of capability (e.g., HEATING, COOLING)
    /// </summary>
    [JsonPropertyName("type")]
    public string? PurpleType { get; set; }

    /// <summary>
    /// The temperature-related capabilities
    /// </summary>
    [JsonPropertyName("temperatures")]
    public Temperatures? Temperatures { get; set; }
}
