using System.Text.Json.Serialization;

namespace KoenZomers.Tado.Api.Models.Authentication;

/// <summary>
/// Contains the response from the Tado API when having performed the device authentication flow
/// </summary>
public class Token
{
    /// <summary>
    /// The access token issued by the authorization server
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// The lifetime in seconds of the access token.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }

    private DateTime? _expiresAt;
    /// <summary>
    /// Date and time at which this token expires
    /// </summary>
    [JsonIgnore]
    public DateTime? ExpiresAt => _expiresAt ??= (ExpiresIn.HasValue ? DateTime.Now.AddSeconds(ExpiresIn.Value) : null);

    /// <summary>
    /// The token that can be used to obtain new access tokens using a refresh flow
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The scope of the access token
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    /// <summary>
    /// The type of the token issued
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    /// <summary>
    /// The identifier of the user associated with the token
    /// </summary>
    [JsonPropertyName("userId")]
    public string? UserId { get; set; }
}
