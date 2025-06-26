using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Entities;

/// <summary>
/// Contains activity data points such as heating power
/// </summary>
public class ActivityDataPoints
{
    /// <summary>
    /// The heating power data point
    /// </summary>
    [JsonPropertyName("heatingPower")]
    public HeatingPower? HeatingPower { get; set; }
}
