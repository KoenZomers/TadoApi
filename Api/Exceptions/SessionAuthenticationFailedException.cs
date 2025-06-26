
namespace KoenZomers.Tado.Api.Exceptions;

/// <summary>
/// Exception thrown when authenticating failed
/// </summary>
public class SessionAuthenticationFailedException : Exception
{
    private const string defaultMessage = "Failed to authenticate session";

    public SessionAuthenticationFailedException() : base(defaultMessage)
    {
    }

    public SessionAuthenticationFailedException(Exception innerException, string message = defaultMessage) : base(message, innerException)
    {
    }
}
