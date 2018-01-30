using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KoenZomers.Tado.Api
{
    public class Session : IDisposable
    {
        #region Properties

        /// <summary>
        /// Username to use to connect to the Tado API. Set by providing it in the constructor.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Password to use to connect to the Tado API. Set by providing it in the constructor.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Base Uri with which all Tado API requests start
        /// </summary>
        public Uri TadoApiBaseUrl => new Uri("https://my.tado.com/api/v2/");

        /// <summary>
        /// Tado API Uri to authenticate against
        /// </summary>
        public Uri TadoApiAuthUrl => new Uri("https://auth.tado.com/oauth/token");

        /// <summary>
        /// Tado API Client Id to use for the OAuth token
        /// </summary>
        public string ClientId => "tado-web-app";

        /// <summary>
        /// Tado API Client Secret to use for the OAuth token
        /// </summary>
        public string ClientSecret => "wZaRN7rpjn3FoNyF5IFuxg9uMzYJcvOoQ8QWiIqS3hfk6gLhVlG57j5YNoZL2Rtc";

        /// <summary>
        /// Allows setting an User Agent which will be provided to the Tado API
        /// </summary>
        public string UserAgent => "";

        private IWebProxy proxyConfiguration;
        /// <summary>
        /// If provided, this proxy will be used for communication with the Tado API. If not provided, no proxy will be used.
        /// </summary>
        public IWebProxy ProxyConfiguration
        {
            get { return proxyConfiguration; }
            set { proxyConfiguration = value; CreateHttpClient(); }
        }

        private NetworkCredential proxyCredential;
        /// <summary>
        /// If provided along with a proxy configuration, these credentials will be used to authenticate to the proxy. If omitted, the default system credentials will be used.
        /// </summary>
        public NetworkCredential ProxyCredential
        {
            get { return proxyCredential; }
            set { proxyCredential = value; CreateHttpClient(); }
        }

        /// <summary>
        /// Boolean indicating if the current session is authenticated
        /// </summary>
        public bool IsAuthenticated => AuthenticatedSession != null;

        /// <summary>
        /// Authenticated Session that will be used to communicate with the Tado API
        /// </summary>
        public Entities.Session AuthenticatedSession { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// HttpClient to use for network communications towards the Tado API
        /// </summary>
        private HttpClient httpClient;

        #endregion

        #region Constructors \ Destructors

        /// <summary>
        /// Initiates a new session to the Tado API
        /// </summary>
        public Session(string username, string password)
        {
            Username = username;
            Password = password;

            // Add a HttpClient to the session to allow for network communication
            httpClient = CreateHttpClient();
        }

        /// <summary>
        /// Clean up
        /// </summary>
        public void Dispose()
        {
            if (httpClient != null)
            {
                httpClient.Dispose();
            }
        }

        #endregion

        #region Authentication Methods

        /// <summary>
        /// Validates if the session has authenticated already and if so, ensures the AccessToken coming forward from the authentication is still valid and assigns it to the HttpClient in this session
        /// </summary>
        private async Task EnsureAccessToken()
        {
            // Check if we have an authenticated session
            if (AuthenticatedSession == null || !AuthenticatedSession.Expires.HasValue)
            {
                // Session is not yet authenticated, nothing we can do at this point
                return;
            }

            // We have an authenticated session, check if its still valid
            if (AuthenticatedSession.Expires.Value < DateTime.Now)
            {
                // Access token is no longer valid, request a new one
                if (!string.IsNullOrEmpty(AuthenticatedSession.RefreshToken))
                {
                    // We have a refresh token, use that to get a new access token
                    AuthenticatedSession = await GetRefreshedSession();
                }
                else
                {
                    // We don't have a refresh token, just get a new access token
                    AuthenticatedSession = await GetNewSession();
                }
            }

            // Set the access token on the HttpClient
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AuthenticatedSession.AccessToken);
            return;
        }

        /// <summary>
        /// Ensures the current session is authenticated or throws an exception if it's not
        /// </summary>
        private void EnsureAuthenticatedSession()
        {
            if (!IsAuthenticated)
            {
                throw new Exceptions.SessionNotAuthenticatedException();
            }
        }

        /// <summary>
        /// Sets up a new session with the Tado API
        /// </summary>
        /// <returns>Session instance</returns>
        private async Task<Entities.Session> GetNewSession()
        {
            // Build the POST body with the authentication arguments
            var queryBuilder = new Helpers.QueryStringBuilder();
            queryBuilder.Add("client_id", ClientId);
            queryBuilder.Add("grant_type", "password");
            queryBuilder.Add("client_secret", ClientSecret);
            queryBuilder.Add("password", Password);
            queryBuilder.Add("scope", "home.user");
            queryBuilder.Add("username", Username);

            return await PostMessageGetResponse<Entities.Session>(TadoApiAuthUrl, queryBuilder);
        }

        /// <summary>
        /// Sets up a session with the Tado API based on the refresh token
        /// </summary>
        /// <returns>Session instance</returns>
        private async Task<Entities.Session> GetRefreshedSession()
        {
            // Build the POST body with the authentication arguments
            var queryBuilder = new Helpers.QueryStringBuilder();
            queryBuilder.Add("client_id", ClientId);
            queryBuilder.Add("grant_type", "refresh_token");
            queryBuilder.Add("client_secret", ClientSecret);
            queryBuilder.Add("refresh_token", AuthenticatedSession.RefreshToken);
            queryBuilder.Add("scope", "home.user");

            return await PostMessageGetResponse<Entities.Session>(TadoApiAuthUrl, queryBuilder);
        }

        /// <summary>
        /// Authenticates this session with the Tado API
        /// </summary>
        public async Task Authenticate()
        {
            try
            {
                // Request the OAuth token
                AuthenticatedSession = await GetNewSession();
            }
            catch (Exception ex)
            {
                throw new Exceptions.SessionAuthenticationFailedException(ex);
            }
        }

        #endregion

        #region Network Traffic Methods

        /// <summary>
        /// Instantiates a new HttpClient preconfigured for use. Note that the caller is responsible for disposing this object.
        /// </summary>
        /// <returns>HttpClient instance</returns>
        private HttpClient CreateHttpClient()
        {
            // Define the HttpClient settings
            var httpClientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = ProxyCredential == null,
                Proxy = ProxyConfiguration,
            };
            if(ProxyConfiguration != null)
            {
                httpClientHandler.UseProxy = true;
            }

            // Check if we need specific credentials for the proxy
            if (ProxyCredential != null && httpClientHandler.Proxy != null)
            {
                httpClientHandler.Proxy.Credentials = ProxyCredential;
            }

            // Create the new HTTP Client
            var httpClient = new HttpClient(httpClientHandler);
            
            if (!string.IsNullOrEmpty(UserAgent))
            {
                // Identify to the server with a specific User Agent
                httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            }
            
            return httpClient;
        }

        /// <summary>
        /// Sends a HTTP POST to the provided uri
        /// </summary>
        /// <param name="queryBuilder">The querystring parameters to send in the POST body</param>
        /// <typeparam name="T">Type of object to try to parse the response JSON into</typeparam>
        /// <param name="uri">Uri of the webservice to send the message to</param>
        /// <returns>Object of type T with the parsed response</returns>
        private async Task<T> PostMessageGetResponse<T>(Uri uri, Helpers.QueryStringBuilder queryBuilder)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("Uri has not been provided");
            }

            // Ensure a valid OAuth token is set on the HttpClient if possible
            await EnsureAccessToken();

            // Prepare the content to POST
            using (var content = new StringContent(queryBuilder.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"))
            {
                // Construct the message towards the webservice
                using (var request = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    // Set the content to send along in the message body with the request
                    request.Content = content;

                    try
                    {
                        // Request the response from the webservice
                        var response = await httpClient.SendAsync(request);
                        var responseBody = await response.Content.ReadAsStringAsync();

                        // Verify if the request was successful (response status 200-299)
                        if (response.IsSuccessStatusCode)
                        {
                            // Request was successful
                            var responseEntity = JsonConvert.DeserializeObject<T>(responseBody);
                            return responseEntity;
                        }
                    }
                    catch(Exception ex)
                    {
                        throw new Exceptions.RequestFailedException(uri, ex);
                    }

                    // Request was not successful. throw an exception
                    throw new Exceptions.RequestFailedException(uri);
                }
            }
        }

        /// <summary>
        /// Sends a message to the Tado API and returns the provided object of type T with the response
        /// </summary>
        /// <typeparam name="T">Object type of the expected response</typeparam>
        /// <param name="uri">Uri of the webservice to send the message to</param>
        /// <param name="expectedHttpStatusCode">The expected Http result status code. Optional. If provided and the webservice returns a different response, the return type will be NULL to indicate failure.</param>
        /// <returns>Typed entity with the result from the webservice</returns>
        protected virtual async Task<T> GetMessageReturnResponse<T>(Uri uri, HttpStatusCode? expectedHttpStatusCode = null)
        {
            // Ensure a valid OAuth token is set on the HttpClient if possible
            await EnsureAccessToken();

            // Construct the request towards the webservice
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                try
                {
                    // Request the response from the webservice
                    using (var response = await httpClient.SendAsync(request))
                    {
                        if (!expectedHttpStatusCode.HasValue || (expectedHttpStatusCode.HasValue && response != null && response.StatusCode == expectedHttpStatusCode.Value))
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var responseEntity = JsonConvert.DeserializeObject<T>(responseString);
                            return responseEntity;
                        }
                        return default(T);
                    }
                }
                catch (Exception ex)
                {
                    // Request was not successful. throw an exception
                    throw new Exceptions.RequestFailedException(uri, ex);
                }
            }
        }

        /// <summary>
        /// Sends a message to the Tado API and returns the provided object of type T with the response
        /// </summary>
        /// <typeparam name="T">Object type of the expected response</typeparam>
        /// <param name="uri">Uri of the webservice to send the message to</param>
        /// <param name="bodyText">Text to send to the webservice in the body</param>
        /// <param name="httpMethod">Http Method to use to connect to the webservice</param>
        /// <param name="expectedHttpStatusCode">The expected Http result status code. Optional. If provided and the webservice returns a different response, the return type will be NULL to indicate failure.</param>
        /// <returns>Typed entity with the result from the webservice</returns>
        protected virtual async Task<T> SendMessageReturnResponse<T>(string bodyText, HttpMethod httpMethod, Uri uri, HttpStatusCode? expectedHttpStatusCode = null)
        {
            // Ensure a valid OAuth token is set on the HttpClient if possible
            await EnsureAccessToken();

            // Load the content to upload
            using (var content = new StringContent(bodyText ?? "", Encoding.UTF8, "application/json"))
            {
                // Construct the message towards the webservice
                using (var request = new HttpRequestMessage(httpMethod, uri))
                {
                    // Check if a body to send along with the request has been provided
                    if (!string.IsNullOrEmpty(bodyText) && httpMethod != HttpMethod.Get)
                    {
                        // Set the content to send along in the message body with the request
                        request.Content = content;
                    }

                    try
                    {
                        // Request the response from the webservice
                        using (var response = await httpClient.SendAsync(request))
                        {
                            if (!expectedHttpStatusCode.HasValue || (expectedHttpStatusCode.HasValue && response != null && response.StatusCode == expectedHttpStatusCode.Value))
                            {
                                var responseString = await response.Content.ReadAsStringAsync();
                                var responseEntity = JsonConvert.DeserializeObject<T>(responseString);
                                return responseEntity;
                            }
                            return default(T);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Request was not successful. throw an exception
                        throw new Exceptions.RequestFailedException(uri, ex);
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns information about the user currently connected through the Tado API
        /// </summary>
        /// <returns>Information about the current user</returns>
        public async Task<Entities.Me> GetMe()
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.Me>(new Uri(TadoApiBaseUrl, "me"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the zones configured in the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <returns>The configured zones</returns>
        public async Task<Entities.Zone[]> GetZones(int homeId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.Zone[]>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/zones"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the devices configured in the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <returns>The configured devices</returns>
        public async Task<Entities.Device[]> GetDevices(int homeId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.Device[]>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/devices"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the mobile devices connected to the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <returns>The connected mobile devices</returns>
        public async Task<Entities.MobileDevice.Item[]> GetMobileDevices(int homeId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.MobileDevice.Item[]>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/mobileDevices"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the installations in the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <returns>The installations</returns>
        public async Task<Entities.Installation[]> GetInstallations(int homeId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.Installation[]>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/installations"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the state of the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <returns>The state of the home</returns>
        public async Task<Entities.HomeState> GetHomeState(int homeId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.HomeState>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/state"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the state of a zone in the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <param name="zoneId">Id of the zone to query</param>
        /// <returns>The state of the zone</returns>
        public async Task<Entities.State> GetZoneState(int homeId, int zoneId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.State>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/zones/{zoneId}/state"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the summarized state of a zone in the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <param name="zoneId">Id of the zone to query</param>
        /// <returns>The summarized state of the zone</returns>
        public async Task<Entities.ZoneSummary> GetSummarizedZoneState(int homeId, int zoneId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.ZoneSummary>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/zones/{zoneId}/overlay"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Returns the current weather at the home with the provided Id from the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to query</param>
        /// <returns>The current weater at the home</returns>
        public async Task<Entities.Weather> GetWeather(int homeId)
        {
            EnsureAuthenticatedSession();

            var response = await GetMessageReturnResponse<Entities.Weather>(new Uri(TadoApiBaseUrl, $"homes/{homeId}/weather"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Sets the temperature in a zone in the home with the provided Id through the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to set the temperature of</param>
        /// <param name="zoneId">Id of the zone to set the temperature of</param>
        /// <param name="temperature">Temperature to set the zone to</param>
        /// <returns>The summarized new state of the zone</returns>
        public async Task<Entities.ZoneSummary> SetTemperatureCelsius(int homeId, int zoneId, double temperature)
        {
            EnsureAuthenticatedSession();

            var overlay = new Entities.Overlay
            {
                Setting = new Entities.Setting
                {
                    Power = "ON",
                    Temperature = new Entities.Temperature
                    {
                        Celsius = temperature
                    },
                    CurrentType = "HEATING"
                },
                Termination = new Entities.Termination
                {
                    CurrentType = "MANUAL"
                }
            };
            var request = JsonConvert.SerializeObject(overlay);

            var response = await SendMessageReturnResponse<Entities.ZoneSummary>(request, HttpMethod.Put, new Uri(TadoApiBaseUrl, $"homes/{homeId}/zones/{zoneId}/overlay"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Sets the temperature in a zone in the home with the provided Id through the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to set the temperature of</param>
        /// <param name="zoneId">Id of the zone to set the temperature of</param>
        /// <param name="temperature">Temperature to set the zone to</param>
        /// <returns>The summarized new state of the zone</returns>
        public async Task<Entities.ZoneSummary> SetTemperatureFahrenheit(int homeId, int zoneId, double temperature)
        {
            EnsureAuthenticatedSession();

            var overlay = new Entities.Overlay
            {
                Setting = new Entities.Setting
                {
                    Power = "ON",
                    Temperature = new Entities.Temperature
                    {
                        Fahrenheit = temperature
                    },
                    CurrentType = "HEATING"
                },
                Termination = new Entities.Termination
                {
                    CurrentType = "MANUAL"
                }
            };
            var request = JsonConvert.SerializeObject(overlay);

            var response = await SendMessageReturnResponse<Entities.ZoneSummary>(request, HttpMethod.Put, new Uri(TadoApiBaseUrl, $"homes/{homeId}/zones/{zoneId}/overlay"), HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Switches the heating off in a zone in the home with the provided Id through the Tado API
        /// </summary>
        /// <param name="homeId">Id of the home to switch the heating off in</param>
        /// <param name="zoneId">Id of the zone to switch the heating off in</param>
        /// <returns>The summarized new state of the zone</returns>
        public async Task<Entities.ZoneSummary> SwitchHeatingOff(int homeId, int zoneId)
        {
            EnsureAuthenticatedSession();

            var overlay = new Entities.Overlay
            {
                Setting = new Entities.Setting
                {
                    Power = "OFF",
                    CurrentType = "HEATING"
                },
                Termination = new Entities.Termination
                {
                    CurrentType = "MANUAL"
                }
            };
            var request = JsonConvert.SerializeObject(overlay);

            var response = await SendMessageReturnResponse<Entities.ZoneSummary>(request, HttpMethod.Put, new Uri(TadoApiBaseUrl, $"homes/{homeId}/zones/{zoneId}/overlay"), HttpStatusCode.OK);
            return response;
        }

        #endregion
    }
}