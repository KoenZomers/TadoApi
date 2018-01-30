using Newtonsoft.Json;

namespace KoenZomers.Tado.Api.Entities.MobileDevice
{
    /// <summary>
    /// Contains the coordinates relative to the home location where Tado is being used
    /// </summary>
    public partial class BearingFromHome
    {
        [JsonProperty("degrees")]
        public double Degrees { get; set; }

        [JsonProperty("radians")]
        public double Radians { get; set; }
    }
}
