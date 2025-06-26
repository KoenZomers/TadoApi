using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models.MobileDevice;

/// <summary>
/// Contains the coordinates relative to the home location where Tado is being used
/// </summary>
public partial class BearingFromHome
{
    /// <summary>
    /// The direction in degrees from the home location
    /// </summary>
    [JsonPropertyName("degrees")]
    public double? Degrees { get; set; }

    /// <summary>
    /// The direction in radians from the home location
    /// </summary>
    [JsonPropertyName("radians")]
    public double? Radians { get; set; }
}
