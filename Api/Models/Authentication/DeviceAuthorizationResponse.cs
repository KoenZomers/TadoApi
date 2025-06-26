using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models.Authentication;

/// <summary>
/// Message returned by the Tado API when requesting a device authorization code
/// </summary>
public class DeviceAuthorizationResponse
{
    /// <summary>
    /// The device code used to initiate the device authorization flow.
    /// </summary>
    [JsonPropertyName("device_code")]
    public string? DeviceCode { get; set; }

    /// <summary>
    /// The number of seconds before the device code expires.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public short? ExpiresIn { get; set; }

    /// <summary>
    /// The interval in seconds at which the client should poll the token endpoint.
    /// </summary>
    [JsonPropertyName("interval")]
    public short? Interval { get; set; }

    /// <summary>
    /// The user code that the user must enter to authorize the device.
    /// </summary>
    [JsonPropertyName("user_code")]
    public string? UserCode { get; set; }

    /// <summary>
    /// The URI where the user should go to authorize the device.
    /// </summary>
    [JsonPropertyName("verification_uri")]
    public string? VerificationUri { get; set; }

    /// <summary>
    /// The complete URI including the user code for user convenience.
    /// </summary>
    [JsonPropertyName("verification_uri_complete")]
    public string? VerificationUriComplete { get; set; }
}
