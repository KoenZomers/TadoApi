using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Contains information about a user
    /// </summary>
    public class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("homes")]
        public Home[] Homes { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("mobileDevices")]
        public MobileDevice.Item[] MobileDevices { get; set; }
    }
}