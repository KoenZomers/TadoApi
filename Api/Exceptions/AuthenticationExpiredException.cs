namespace KoenZomers.Tado.Api.Exceptions;

/// <summary>
/// Exception thrown when the authentication session expired
/// </summary>
public class AuthenticationExpiredException : Exception
{
    private const string DefaultMessage = "The authentication has expired. You have to reauthenticate.";

    public AuthenticationExpiredException(string? message) : base(message ?? DefaultMessage)
    {
    }

    public AuthenticationExpiredException(Exception innerException, string? message) : base(message ?? DefaultMessage, innerException)
    {
    }
}
