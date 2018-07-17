using Newtonsoft.Json;
using System;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Information about when the current state of the Tado device is expected to change
    /// </summary>
    public partial class Termination
    {
        /// <summary>
        /// Defines if and what will make the Tado device change its state
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(Converters.DurationModeConverter))]
        public Enums.DurationModes? CurrentType { get; set; }

        /// <summary>
        /// Date and time at which the termination mode is expected to change. NULL if CurrentType is Manual thus impossible to predict when the next state change will be.
        /// </summary>
        [JsonProperty("projectedExpiry", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ProjectedExpiry { get; set; }

        /// <summary>
        /// Date and time at which the termination mode will change. NULL if CurrentType is Manual thus impossible to predict when the next state change will be.
        /// </summary>
        [JsonProperty("expiry", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Expiry { get; set; }

        /// <summary>
        /// Amount of seconds remaining before the Tado device will change its state. Will only contain a value if CurrentType is Timer.
        /// </summary>
        [JsonProperty("durationInSeconds", NullValueHandling = NullValueHandling.Ignore)]
        public int? DurationInSeconds { get; set; }
    }
}