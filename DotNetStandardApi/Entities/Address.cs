using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities
{
    public partial class Address
    {
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("addressLine2")]
        public object AddressLine2 { get; set; }

        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public object State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}