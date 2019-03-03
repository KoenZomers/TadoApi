using System;

namespace KoenZomers.Tado.Api.Exceptions
{
    /// <summary>
    /// Exception thrown when a request failed
    /// </summary>
    public class RequestFailedException : Exception
    {
        /// <summary>
        /// Uri that was called
        /// </summary>
        public Uri Uri { get; private set; }

        public RequestFailedException(Uri uri) : base("A request failed")
        {
            Uri = uri;
        }

        public RequestFailedException(Uri uri, Exception innerException) : base("A request failed", innerException)
        {
            Uri = uri;
        }
    }
}
