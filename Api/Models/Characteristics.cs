using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Entities;

/// <summary>
/// Characteristics of a device
/// </summary>
public class Characteristics
{
    /// <summary>
    /// The list of capabilities supported by the device
    /// </summary>
    [JsonPropertyName("capabilities")]
    public string[]? Capabilities { get; set; }
}
