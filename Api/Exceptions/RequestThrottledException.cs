namespace KoenZomers.Tado.Api.Exceptions;

/// <summary>
/// Exception thrown when a request is being throttled (HTTP 429 response)
/// </summary>
public class RequestThrottledException : Exception
{
    /// <summary>
    /// Uri that was called
    /// </summary>
    public Uri Uri { get; private set; }

    /// <summary>
    /// The rate limit policy name (e.g., "perday")
    /// </summary>
    public string? RateLimitPolicyName { get; private set; }

    /// <summary>
    /// The quota limit for the rate limit policy (e.g., 20000 requests per day)
    /// </summary>
    public int? RateLimitQuota { get; private set; }

    /// <summary>
    /// The time window for the rate limit policy in seconds (e.g., 86400 for daily)
    /// </summary>
    public int? RateLimitWindow { get; private set; }

    /// <summary>
    /// The remaining requests allowed in the current window
    /// </summary>
    public int? RemainingRequests { get; private set; }

    /// <summary>
    /// The time in seconds until the rate limit resets
    /// </summary>
    public int? ResetTimeSeconds { get; private set; }

    /// <summary>
    /// Instantiates a new instance of the <see cref="RequestThrottledException"/> class.
    /// </summary>
    /// <param name="uri">Uri that was being called</param>
    /// <param name="httpResponseMessage">Http Response Message. Optional.</param>
    /// <param name="innerException">Exception raised while making the request. Optional.</param>
    public RequestThrottledException(Uri uri, HttpResponseMessage? httpResponseMessage = null, Exception? innerException = null) : base($"The request to {uri} failed because of throttling", innerException)
    {
        Uri = uri;
            
        if (httpResponseMessage != null)
        {
            ParseRateLimitHeaders(httpResponseMessage);
        }
    }

    /// <summary>
    /// Parses the RateLimit-Policy and RateLimit headers from the HTTP response
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message containing the headers</param>
    private void ParseRateLimitHeaders(HttpResponseMessage httpResponseMessage)
    {
        // Parse RateLimit-Policy header: "perday";q=20000;w=86400
        if (httpResponseMessage.Headers.TryGetValues("RateLimit-Policy", out var policyValues))
        {
            var policyHeader = policyValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(policyHeader))
            {
                ParseRateLimitPolicy(policyHeader);
            }
        }

        // Parse RateLimit header: "perday";r=0;t=7082
        if (httpResponseMessage.Headers.TryGetValues("RateLimit", out var rateLimitValues))
        {
            var rateLimitHeader = rateLimitValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(rateLimitHeader))
            {
                ParseRateLimit(rateLimitHeader);
            }
        }
    }

    /// <summary>
    /// Parses the RateLimit-Policy header to extract policy name, quota, and window
    /// Format: "policy_name";q=quota;w=window_seconds
    /// </summary>
    /// <param name="policyHeader">The RateLimit-Policy header value</param>
    private void ParseRateLimitPolicy(string policyHeader)
    {
        var parts = policyHeader.Split(';');
            
        // First part is the policy name (remove quotes)
        if (parts.Length > 0)
        {
            RateLimitPolicyName = parts[0].Trim('"');
        }

        // Parse remaining parts for quota (q) and window (w)
        foreach (var part in parts.Skip(1))
        {
            var trimmedPart = part.Trim();
            if (trimmedPart.StartsWith("q=") && int.TryParse(trimmedPart.Substring(2), out var quota))
            {
                RateLimitQuota = quota;
            }
            else if (trimmedPart.StartsWith("w=") && int.TryParse(trimmedPart.Substring(2), out var window))
            {
                RateLimitWindow = window;
            }
        }
    }

    /// <summary>
    /// Parses the RateLimit header to extract remaining requests and reset time
    /// Format: "policy_name";r=remaining;t=reset_seconds
    /// </summary>
    /// <param name="rateLimitHeader">The RateLimit header value</param>
    private void ParseRateLimit(string rateLimitHeader)
    {
        var parts = rateLimitHeader.Split(';');

        // Parse parts for remaining requests (r) and reset time (t)
        foreach (var part in parts.Skip(1)) // Skip policy name
        {
            var trimmedPart = part.Trim();
            if (trimmedPart.StartsWith("r=") && int.TryParse(trimmedPart.Substring(2), out var remaining))
            {
                RemainingRequests = remaining;
            }
            else if (trimmedPart.StartsWith("t=") && int.TryParse(trimmedPart.Substring(2), out var resetTime))
            {
                ResetTimeSeconds = resetTime;
            }
        }
    }
}