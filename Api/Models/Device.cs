using KoenZomers.Tado.Api.Entities;
using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Information about one Tado device
/// </summary>
public class Device
{
    /// <summary>
    /// The type of the device (e.g., SMART_THERMOSTAT)
    /// </summary>
    [JsonPropertyName("deviceType")]
    public string? DeviceType { get; set; }

    /// <summary>
    /// The full serial number of the device
    /// </summary>
    [JsonPropertyName("serialNo")]
    public string? SerialNo { get; set; }

    /// <summary>
    /// The short version of the device's serial number
    /// </summary>
    [JsonPropertyName("shortSerialNo")]
    public string? ShortSerialNo { get; set; }

    /// <summary>
    /// The current firmware version installed on the device
    /// </summary>
    [JsonPropertyName("currentFwVersion")]
    public string? CurrentFwVersion { get; set; }

    /// <summary>
    /// The current connection state of the device
    /// </summary>
    [JsonPropertyName("connectionState")]
    public ConnectionState? ConnectionState { get; set; }

    /// <summary>
    /// The characteristics of the device
    /// </summary>
    [JsonPropertyName("characteristics")]
    public Characteristics? Characteristics { get; set; }

    /// <summary>
    /// The list of duties assigned to the device
    /// </summary>
    [JsonPropertyName("duties")]
    public string[]? Duties { get; set; }

    /// <summary>
    /// The mounting state of the device
    /// </summary>
    [JsonPropertyName("mountingState")]
    public MountingState? MountingState { get; set; }

    /// <summary>
    /// The battery state of the device (e.g., NORMAL, LOW)
    /// </summary>
    [JsonPropertyName("batteryState")]
    public string? BatteryState { get; set; }

    /// <summary>
    /// Indicates if child lock is enabled or disabled on the Tado device
    /// </summary>
    [JsonPropertyName("childLockEnabled")]
    public bool? ChildLockEnabled { get; set; }
}
