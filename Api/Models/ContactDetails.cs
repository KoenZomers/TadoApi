using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Contains contact details of an owner of a house
/// </summary>
public class ContactDetails
{
    /// <summary>
    /// The full name of the contact person
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The email address of the contact person
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The phone number of the contact person
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}
