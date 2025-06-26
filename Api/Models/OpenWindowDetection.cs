using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Open Window Detection settings
/// </summary>
public partial class OpenWindowDetection
{
    /// <summary>
    /// Indicates whether the open window detection feature is supported
    /// </summary>
    [JsonPropertyName("supported")]
    public bool? Supported { get; set; }

    /// <summary>
    /// Indicates whether the open window detection feature is enabled
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// The timeout duration in seconds for open window detection
    /// </summary>
    [JsonPropertyName("timeoutInSeconds")]
    public long? TimeoutInSeconds { get; set; }
}
