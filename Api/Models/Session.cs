using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models;

/// <summary>
/// Session token coming from an OAuth authentication request
/// </summary>
public class Session
{
    /// <summary>
    /// The access token used for authenticated requests
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    private int? expiresIn;

    /// <summary>
    /// The number of seconds until the access token expires
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn
    {
        get => expiresIn;
        set
        {
            expiresIn = value;
            Expires = value.HasValue ? DateTime.Now.AddSeconds(value.Value) : null;
        }
    }

    /// <summary>
    /// Date and time at which the access token expires
    /// </summary>
    public DateTime? Expires { get; private set; }

    /// <summary>
    /// The unique token identifier
    /// </summary>
    [JsonPropertyName("jti")]
    public string? Jti { get; set; }

    /// <summary>
    /// The refresh token used to obtain a new access token
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The scope of access granted by the token
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    /// <summary>
    /// The type of token (typically "Bearer")
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
}
