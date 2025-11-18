using KoenZomers.Tado.Api.Entities;
using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Contains detailed information about a house
/// </summary>
public class House
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("dateTimeZone")]
    public string DateTimeZone { get; set; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("temperatureUnit")]
    public string TemperatureUnit { get; set; }

    [JsonPropertyName("installationCompleted")]
    public bool InstallationCompleted { get; set; }

    [JsonPropertyName("partner")]
    public object Partner { get; set; }

    [JsonPropertyName("simpleSmartScheduleEnabled")]
    public bool SimpleSmartScheduleEnabled { get; set; }

    [JsonPropertyName("awayRadiusInMeters")]
    public double AwayRadiusInMeters { get; set; }

    [JsonPropertyName("license")]
    public string License { get; set; }

    [JsonPropertyName("christmasModeEnabled")]
    public bool ChristmasModeEnabled { get; set; }

    [JsonPropertyName("contactDetails")]
    public ContactDetails ContactDetails { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("geolocation")]
    public Geolocation Geolocation { get; set; }
}