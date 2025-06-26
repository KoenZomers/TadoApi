using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Indicates whether the Early Start feature is enabled
/// </summary>
public class EarlyStart
{
    /// <summary>
    /// Whether Early Start is enabled for the schedule
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
}
