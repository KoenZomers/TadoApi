using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// The current state of a Tado device
/// </summary>
public partial class Setting
{
    /// <summary>
    /// Type of Tado device
    /// </summary>
    [JsonPropertyName("type")]
    [JsonConverter(typeof(Converters.DeviceTypeConverter))] // Ensure this is a System.Text.Json converter
    public Enums.DeviceTypes? DeviceType { get; set; }

    /// <summary>
    /// The power state of the Tado device
    /// </summary>
    [JsonPropertyName("power")]
    [JsonConverter(typeof(Converters.PowerStatesConverter))] // Ensure this is a System.Text.Json converter
    public Enums.PowerStates? Power { get; set; }

    /// <summary>
    /// The temperature the Tado device is set to change the zone to
    /// </summary>
    [JsonPropertyName("temperature")]
    public Temperature? Temperature { get; set; }
}
