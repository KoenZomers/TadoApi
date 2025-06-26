namespace KoenZomers.Tado.Api.Exceptions;

/// <summary>
/// Exception thrown when the authentication session expired
/// </summary>
public class AuthenticationExpiredException : Exception
{
    private const string defaultMessage = "The authentication has expired. You have to reauthenticate.";

    public AuthenticationExpiredException() : base(defaultMessage)
    {
    }

    public AuthenticationExpiredException(Exception innerException, string message = defaultMessage) : base(message, innerException)
    {
    }
}
