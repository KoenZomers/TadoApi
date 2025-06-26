using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Contains information about a user
/// </summary>
public class User
{
    /// <summary>
    /// The full name of the user
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The email address of the user
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The username used by the user
    /// </summary>
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    /// <summary>
    /// The unique identifier of the user
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The list of homes associated with the user
    /// </summary>
    [JsonPropertyName("homes")]
    public Home[]? Homes { get; set; }

    /// <summary>
    /// The locale or language preference of the user
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// The list of mobile devices linked to the user
    /// </summary>
    [JsonPropertyName("mobileDevices")]
    public MobileDevice.Item[]? MobileDevices { get; set; }
}
