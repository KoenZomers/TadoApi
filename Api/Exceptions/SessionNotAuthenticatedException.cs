using System;

namespace KoenZomers.Tado.Api.Exceptions
{
    /// <summary>
    /// Exception thrown when functionality is called which requires the session to be authenticated while it isn't yet
    /// </summary>
    public class SessionNotAuthenticatedException : Exception
    {
        private const string defaultMessage = "This session has not yet been authenticated.Please call Authenticate() first.";

        public SessionNotAuthenticatedException() : base(defaultMessage)
        {
        }

        public SessionNotAuthenticatedException(Exception innerException, string message = defaultMessage) : base(message, innerException)
        {
        }
    }
}
