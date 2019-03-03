using Newtonsoft.Json;
using System;

namespace KoenZomers.Tado.Api.Entities
{
    /// <summary>
    /// Session token coming forward from an OAuth authentication request
    /// </summary>
    public class Session
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        private int? expiresIn;
        [JsonProperty(PropertyName = "expires_in")]
        public int? ExpiresIn
        {
            get { return expiresIn; }
            set
            {
                expiresIn = value;
                Expires = value.HasValue ? (DateTime?) DateTime.Now.AddSeconds(value.Value) : null;
            }
        }

        /// <summary>
        /// Date and time at which the Access Token expires
        /// </summary>
        public DateTime? Expires { get; private set; }

        [JsonProperty(PropertyName = "jti")]
        public string Jti { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
    }
}
