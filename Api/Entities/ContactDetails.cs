using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Contains contact details of an owner of a house
    /// </summary>
    public class ContactDetails
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}