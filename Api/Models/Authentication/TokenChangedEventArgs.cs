namespace KoenZomers.Tado.Api.Models.Authentication;

/// <summary>
/// EventArgs to pass the new token when it has been changed/refreshed
/// </summary>
public sealed class TokenChangedEventArgs(Token token) : EventArgs
{
    /// <summary>
    /// The new token
    /// </summary>
    public Token Token { get; } = token;
}
