using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Represents the address details of a location
/// </summary>
public partial class Address
{
    /// <summary>
    /// The first line of the address
    /// </summary>
    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }

    /// <summary>
    /// The second line of the address (optional)
    /// </summary>
    [JsonPropertyName("addressLine2")]
    public object? AddressLine2 { get; set; }

    /// <summary>
    /// The postal or ZIP code
    /// </summary>
    [JsonPropertyName("zipCode")]
    public string? ZipCode { get; set; }

    /// <summary>
    /// The city of the address
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>
    /// The state or province (optional)
    /// </summary>
    [JsonPropertyName("state")]
    public object? State { get; set; }

    /// <summary>
    /// The country of the address
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; set; }
}
