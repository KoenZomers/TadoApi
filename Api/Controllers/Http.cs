using KoenZomers.Tado.Api.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace KoenZomers.Tado.Api.Controllers;

/// <summary>
/// Controller wich allows to perform HTTP calls
/// </summary>
public class Http : Base
{
    #region Properties
    
    /// <summary>
    /// Default timeout for HTTP requests in seconds
    /// </summary>
    public short DefaultApiTimeoutSeconds => Configuration?.CurrentValue.DefaultApiTimeoutSeconds ?? 30;

    #endregion

    #region Fields

    /// <summary>
    /// HttpClient to use for network communications towards the Tado API
    /// </summary>
    private readonly HttpClient? TadoHttpClient;

    /// <summary>
    /// Access to the application configuration
    /// </summary>
    private IOptionsMonitor<Configuration.Tado>? Configuration;

    #endregion

    /// <summary>
    /// Instantiate the HTTP controller
    /// </summary>
    /// <param name="httpClientFactory">HttpClientFactory to use to retrieve a HttpClient from</param>
    /// <param name="configuration">The application configuration</param>
    /// <param name="loggerFactory">LoggerFactory to use to retrieve Logger instance from</param>
    public Http(IHttpClientFactory httpClientFactory, IOptionsMonitor<Configuration.Tado> configuration, ILoggerFactory loggerFactory) : base(loggerFactory: loggerFactory)
    {
        Configuration = configuration;
        TadoHttpClient = httpClientFactory.CreateClient("Tado");
        TadoHttpClient.Timeout = TimeSpan.FromSeconds(DefaultApiTimeoutSeconds);
    }

    /// <summary>
    /// Sends a HTTP POST to the provided uri
    /// </summary>
    /// <param name="queryBuilder">The querystring parameters to send in the POST body</param>
    /// <typeparam name="T">Type of object to try to parse the response JSON into</typeparam>
    /// <param name="uri">Uri of the webservice to send the message to</param>
    /// <param name="retryIntervalIfFailed">If provided, in case of a non 2xx response, it will keep retrying the call. Optional, if not provided, it will not retry and throw a <see cref="Exceptions.RequestFailedException"/> Exception if it fails.</param>
    /// <param name="maximumRetries">If provided, in case of a non 2xx response, it will retry the call at most the amount configured through this parameter. Optional, if not provided, it will endlessly retry. If <paramref name="retryIntervalIfFailed"/> has not been set, this is being ignored.</param>
    /// <param name="token">Optional token. If provided, it will be used to authenticate the request. If omitted, it will send the request anonymously.</param>
    /// <returns>Object of type T with the parsed response</returns>
    /// <exception cref="Exceptions.RequestThrottledException">Thrown when the request is getting throttled</exception>
    /// <exception cref="Exceptions.RequestThrottledException">Thrown when the request is getting throttled</exception>
    public async Task<T?> PostMessageGetResponse<T>(Uri uri, QueryStringBuilder queryBuilder, short? retryIntervalIfFailed = null, short? maximumRetries = null, Models.Authentication.Token? token = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(TadoHttpClient);

        var retryCount = 0;
        do
        {
            // Request the response from the webservice
            Logger.LogDebug($"Calling Tado API at {uri} with content: {queryBuilder.ToString()}");

            // Prepare the content to POST
            using var content = new StringContent(queryBuilder.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");

            // Construct the message towards the webservice
            using var request = new HttpRequestMessage(HttpMethod.Post, uri);

            // Check if we should include an Authorization Bearer token
            if (token is not null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            }

            // Set the content to send along in the message body with the request
            request.Content = content;

            retryCount++;
            HttpResponseMessage response;
            Stream responseContentStream;
            try
            {
                response = await TadoHttpClient.SendAsync(request);
                responseContentStream = await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                throw new Exceptions.RequestFailedException(uri, ex);
            }

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                // Request was throttled
                throw new Exceptions.RequestThrottledException(uri, response);
            }
            else if (!response.IsSuccessStatusCode)
            {
                // Request was not successful
                if (!retryIntervalIfFailed.HasValue || (maximumRetries.HasValue && retryCount >= maximumRetries.Value))
                {
                    // We should not retry or we have reached the maximum number of retries
                    throw new Exceptions.RequestFailedException(uri);
                }

                // Pause and retry
                Logger.LogDebug($"Request failed with status code {response.StatusCode} for URI {uri}. Retrying in {retryIntervalIfFailed.Value} seconds...");
                Thread.Sleep(TimeSpan.FromSeconds(retryIntervalIfFailed.Value));
            }
            else
            {
                // Request was successful (response status 200-299)
                var responseEntity = await JsonSerializer.DeserializeAsync<T>(responseContentStream);
                return responseEntity;
            }
        } while (true);
    }

    /// <summary>
    /// Sends a message to the Tado API and returns the provided object of type T with the response
    /// </summary>
    /// <typeparam name="T">Object type of the expected response</typeparam>
    /// <param name="uri">Uri of the webservice to send the message to</param>
    /// <param name="expectedHttpStatusCode">The expected Http result status code. Optional. If provided and the webservice returns a different response, the return type will be NULL to indicate failure.</param>
    /// <param name="token">Optional token. If provided, it will be used to authenticate the request. If omitted, it will send the request anonymously.</param>
    /// <returns>Typed entity with the result from the webservice</returns>
    /// <exception cref="Exceptions.RequestThrottledException">Thrown when the request is getting throttled</exception>
    public async Task<T?> GetMessageReturnResponse<T>(Uri uri, HttpStatusCode? expectedHttpStatusCode = null, Models.Authentication.Token? token = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(TadoHttpClient);

        // Construct the request towards the webservice
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);

        // Check if we should include an Authorization Bearer token
        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        HttpResponseMessage response;
        try
        {
            // Request the response from the webservice
            response = await TadoHttpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            // Request was not successful, throw an exception
            throw new Exceptions.RequestFailedException(uri, ex);
        }

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            // Request was throttled
            throw new Exceptions.RequestThrottledException(uri, response);
        }
        else if (!expectedHttpStatusCode.HasValue || expectedHttpStatusCode.HasValue && response != null && response.StatusCode == expectedHttpStatusCode.Value)
        {
            var responseContentStream = await response.Content.ReadAsStreamAsync();
            var responseEntity = await JsonSerializer.DeserializeAsync<T>(responseContentStream);
            return responseEntity;
        }
        return default;
    }

    /// <summary>
    /// Sends a message to the Tado API and returns the provided object of type T with the response
    /// </summary>
    /// <typeparam name="T">Object type of the expected response</typeparam>
    /// <param name="uri">Uri of the webservice to send the message to</param>
    /// <param name="bodyText">Text to send to the webservice in the body</param>
    /// <param name="httpMethod">Http Method to use to connect to the webservice</param>
    /// <param name="expectedHttpStatusCode">The expected Http result status code. Optional. If provided and the webservice returns a different response, the return type will be NULL to indicate failure.</param>
    /// <param name="token">Optional token. If provided, it will be used to authenticate the request. If omitted, it will send the request anonymously.</param>
    /// <returns>Typed entity with the result from the webservice</returns>
    /// <exception cref="Exceptions.RequestThrottledException">Thrown when the request is getting throttled</exception>
    public async Task<T?> SendMessageReturnResponse<T>(string bodyText, HttpMethod httpMethod, Uri uri, HttpStatusCode? expectedHttpStatusCode = null, Models.Authentication.Token? token = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(TadoHttpClient);

        // Load the content to send in the body
        using var content = new StringContent(bodyText ?? "", Encoding.UTF8, "application/json");

        // Construct the message towards the webservice
        using var request = new HttpRequestMessage(httpMethod, uri);

        // Check if we should include an Authorization Bearer token
        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        // Check if a body to send along with the request has been provided
        if (!string.IsNullOrEmpty(bodyText) && httpMethod != HttpMethod.Get)
        {
            // Set the content to send along in the message body with the request
            request.Content = content;
        }

        HttpResponseMessage response;
        try
        {
            // Request the response from the webservice
            response = await TadoHttpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            // Request was not successful. throw an exception
            throw new Exceptions.RequestFailedException(uri, ex);
        }

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            // Request was throttled
            throw new Exceptions.RequestThrottledException(uri, response);
        }
        else if (!expectedHttpStatusCode.HasValue || expectedHttpStatusCode.HasValue && response != null && response.StatusCode == expectedHttpStatusCode.Value)
        {
            var responseContentStream = await response.Content.ReadAsStreamAsync();
            var responseEntity = await JsonSerializer.DeserializeAsync<T>(responseContentStream);
            return responseEntity;
        }
        return default;
    }

    /// <summary>
    /// Sends a message to the Tado API without looking at the response
    /// </summary>
    /// <param name="uri">Uri of the webservice to send the message to</param>
    /// <param name="bodyText">Text to send to the webservice in the body</param>
    /// <param name="httpMethod">Http Method to use to connect to the webservice</param>
    /// <param name="expectedHttpStatusCode">The expected Http result status code. Optional. If provided and the webservice returns a different response, the return type will be false to indicate failure.</param>
    /// <param name="token">Optional token. If provided, it will be used to authenticate the request. If omitted, it will send the request anonymously.</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    /// <exception cref="Exceptions.RequestThrottledException">Thrown when the request is getting throttled</exception>
    public async Task<bool> SendMessage(string bodyText, HttpMethod httpMethod, Uri uri, HttpStatusCode? expectedHttpStatusCode = null, Models.Authentication.Token? token = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(TadoHttpClient);

        // Load the content to send in the body
        using var content = new StringContent(bodyText ?? "", Encoding.UTF8, "application/json");
        // Construct the message towards the webservice
        using var request = new HttpRequestMessage(httpMethod, uri);

        // Check if we should include an Authorization Bearer token
        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        // Check if a body to send along with the request has been provided
        if (!string.IsNullOrEmpty(bodyText) && httpMethod != HttpMethod.Get)
        {
            // Set the content to send along in the message body with the request
            request.Content = content;
        }

        HttpResponseMessage response;
        try
        {
            // Request the response from the webservice
            response = await TadoHttpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            // Request was not successful. throw an exception
            throw new Exceptions.RequestFailedException(uri, ex);
        }

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            // Request was throttled
            throw new Exceptions.RequestThrottledException(uri, response);
        }
        else if (!expectedHttpStatusCode.HasValue || expectedHttpStatusCode.HasValue && response != null && response.StatusCode == expectedHttpStatusCode.Value)
        {
            return true;
        }
        return false;
    }
}
