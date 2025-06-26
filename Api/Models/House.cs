using KoenZomers.Tado.Api.Entities;
using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Models
{
    /// <summary>
    /// Contains detailed information about a house
    /// </summary>
    public class House
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dateTimeZone")]
        public string DateTimeZone { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("temperatureUnit")]
        public string TemperatureUnit { get; set; }

        [JsonProperty("installationCompleted")]
        public bool InstallationCompleted { get; set; }

        [JsonProperty("partner")]
        public object Partner { get; set; }

        [JsonProperty("simpleSmartScheduleEnabled")]
        public bool SimpleSmartScheduleEnabled { get; set; }

        [JsonProperty("awayRadiusInMeters")]
        public double AwayRadiusInMeters { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("christmasModeEnabled")]
        public bool ChristmasModeEnabled { get; set; }

        [JsonProperty("contactDetails")]
        public ContactDetails ContactDetails { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("geolocation")]
        public Geolocation Geolocation { get; set; }
    }
}