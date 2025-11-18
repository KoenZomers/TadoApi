using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Information about the state of the home
/// </summary>
public class HomeState
{
    [JsonPropertyName("presence")]
    public string Presence { get; set; }
}