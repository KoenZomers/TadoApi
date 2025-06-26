using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace KoenZomers.Tado.Api.Controllers;

/// <summary>
/// Initiates a new session to the Tado API
/// </summary>
public class Tado(Http HttpController, IOptionsMonitor<Configuration.Tado> Configuration, ILoggerFactory loggerFactory) : Base(loggerFactory: loggerFactory)
{
    #region Properties

    /// <summary>
    /// Base Uri with which all Tado API requests start
    /// </summary>
    public Uri TadoApiBaseUrl => new(Configuration?.CurrentValue.BaseUrl ?? "https://my.tado.com/api/v2/");

    /// <summary>
    /// Tado API Uri to authenticate against
    /// </summary>
    public Uri TadoApiAuthUrl => new(Configuration?.CurrentValue?.AuthenticationUrl ?? "https://login.tado.com/oauth2/device_authorize");

    /// <summary>
    /// Tado API Uri to retrieve a token from
    /// </summary>
    public Uri TadoTokenUrl => new(Configuration?.CurrentValue?.TokenUrl ?? "https://login.tado.com/oauth2/token");

    /// <summary>
    /// Tado API Client Id to use for the OAuth token
    /// </summary>
    public string ClientId => Configuration?.CurrentValue?.ClientId ?? "1bb50063-6b0c-4d11-bd99-387f4a91cc46";

    /// <summary>
    /// Boolean indicating if the current session is authenticated
    /// </summary>
    public bool IsAuthenticated => _token is not null && ((_token.ExpiresAt.HasValue && _token.ExpiresAt.Value > DateTime.Now) || !string.IsNullOrWhiteSpace(_token.RefreshToken));

    #endregion

    #region Fields

    /// <summary>
    /// The current token used to authenticate against the Tado API
    /// </summary>
    protected Models.Authentication.Token? _token;

    #endregion

    #region Constructors \ Destructors

    /// <summary>
    /// Clean up
    /// </summary>
    public void Dispose()
    {

    }

    #endregion

    #region Authentication Methods

    /// <summary>
    /// Initiates an authentication session by requesting a device authentication url which can be used to authenticate to Tado
    /// </summary>
    /// <returns>Instance of <see cref="Models.Authentication.DeviceAuthorizationResponse"/> containing the information to perform the device authentication or NULL if unable to initiate authentication session</returns>
    public async Task<Models.Authentication.DeviceAuthorizationResponse?> GetDeviceCodeAuthentication()
    {
        var queryBuilder = new Helpers.QueryStringBuilder();
        queryBuilder.Add("client_id", ClientId);
        queryBuilder.Add("scope", "offline_access");

        Models.Authentication.DeviceAuthorizationResponse? response;
        try
        {
            response = await HttpController.PostMessageGetResponse<Models.Authentication.DeviceAuthorizationResponse>(TadoApiAuthUrl, queryBuilder);
        }
        catch(Exceptions.RequestFailedException e)
        {
            Logger.LogWarning($"Failed to get device code authentication from Tado API at '{e.Uri}'. Response: {e.InnerException?.Message ?? e.Message}");
            return null;
        }

        return response;
    }

    /// <summary>
    /// Waits for the provided device authorization flow to be completed by the user in a browser
    /// </summary>
    /// <param name="deviceAuthorization">The device authorization flow session to wait for to be completed</param>
    /// <returns>TokenResponse containing the token to access to Tado API or NULL if it failed to complete</returns>
    public async Task<Models.Authentication.Token?> WaitForDeviceCodeAuthenticationToComplete(Models.Authentication.DeviceAuthorizationResponse deviceAuthorization)
    {
        ArgumentNullException.ThrowIfNull(deviceAuthorization);
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceAuthorization.DeviceCode);

        var queryBuilder = new Helpers.QueryStringBuilder();
        queryBuilder.Add("client_id", ClientId);
        queryBuilder.Add("device_code", deviceAuthorization.DeviceCode);
        queryBuilder.Add("grant_type", "urn:ietf:params:oauth:grant-type:device_code");

        Models.Authentication.Token? response;
        try
        {
            response = await HttpController.PostMessageGetResponse<Models.Authentication.Token>(TadoTokenUrl,
                                                                                                queryBuilder,
                                                                                                deviceAuthorization.Interval ?? 5,
                                                                                                (short) (deviceAuthorization.ExpiresIn.HasValue ? deviceAuthorization.ExpiresIn.Value / deviceAuthorization.Interval ?? 5 : 60));
        }
        catch (Exceptions.RequestFailedException e)
        {
            Logger.LogWarning($"Failed to get device code authentication from Tado API at '{e.Uri}'. Response: {e.InnerException?.Message ?? e.Message}");
            return null;
        }

        return response;
    }

    /// <summary>
    /// Retrieves an access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>Access token or NULL if unable to retrieve an access token</returns>
    public async Task<Models.Authentication.Token?> GetAccessTokenWithRefreshToken(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        var queryBuilder = new Helpers.QueryStringBuilder();
        queryBuilder.Add("client_id", ClientId);
        queryBuilder.Add("refresh_token", refreshToken);
        queryBuilder.Add("grant_type", "refresh_token");

        Models.Authentication.Token? response;
        try
        {
            response = await HttpController.PostMessageGetResponse<Models.Authentication.Token>(TadoTokenUrl, queryBuilder);
        }
        catch (Exceptions.RequestFailedException e)
        {
            Logger.LogWarning($"Failed to get access token through refresh token from Tado API at '{e.Uri}'. Response: {e.InnerException?.Message ?? e.Message}");
            return null;
        }

        return response;
    }

    /// <summary>
    /// Authenticates this session based on the passed in token
    /// </summary>
    /// <param name="token">Token to use to authenticate</param>
    /// <returns>True if authentication successful, false if it failed</returns>
    public bool Authenticate(Models.Authentication.Token token)
    {
        ArgumentNullException.ThrowIfNull(token);

        _token = token;

        return IsAuthenticated;
    }

    /// <summary>
    /// Tries to ensure we have a valid authentication token to work with
    /// </summary>
    /// <returns>Authentication token</returns>
    /// <exception cref="Exceptions.SessionNotAuthenticatedException">Thrown if the session is not authenticated</exception>
    /// <exception cref="Exceptions.AuthenticationExpiredException">Thrown if the session is no longer auhtenticated and retrieval of a new token through the refesh token failed</exception>
    private async Task<Models.Authentication.Token> EnsureValidToken()
    {
        if (_token is null)
        {
            throw new Exceptions.SessionNotAuthenticatedException();
        }
        if (_token.ExpiresAt.HasValue && _token.ExpiresAt.Value < DateTime.Now && string.IsNullOrWhiteSpace(_token.RefreshToken))
        {
            throw new Exceptions.AuthenticationExpiredException();
        }
        if (_token.ExpiresAt.HasValue && _token.ExpiresAt.Value < DateTime.Now && !string.IsNullOrWhiteSpace(_token.RefreshToken))
        {
            // Token has expired, try to refresh it
            var refreshedToken = await GetAccessTokenWithRefreshToken(_token.RefreshToken);
            if (refreshedToken is null)
            {
                throw new Exceptions.AuthenticationExpiredException();
            }
            _token = refreshedToken;
        }
        return _token;
    }

    #endregion

    #region Data Retrieval

    /// <summary>
    /// Internal method to retrieve typed data from the Tado API
    /// </summary>
    /// <typeparam name="T">Model type to retrieve data for</typeparam>
    /// <param name="endPoint">The endpoint to be called</param>
    /// <param name="expectedStatusCode">The expected response status. If omitted, HTTP 200 OK is assumed.</param>
    /// <returns>The typed data</returns>
    private async Task<T?> GetData<T>(string endPoint, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var token = await EnsureValidToken();

        var response = await HttpController.GetMessageReturnResponse<T>(new Uri(TadoApiBaseUrl, endPoint), expectedStatusCode, token);
        return response;
    }

    /// <summary>
    /// Returns information about the user currently connected through the Tado API
    /// </summary>
    /// <returns>Information about the current user</returns>
    public async Task<Models.User?> GetMe() => await GetData<Models.User>("me");

    /// <summary>
    /// Returns the zones configured in the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The configured zones</returns>
    public async Task<Models.Zone[]?> GetZones(int homeId) => await GetData<Models.Zone[]>($"homes/{homeId}/zones");

    /// <summary>
    /// Returns the devices configured in the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The configured devices</returns>
    public async Task<Models.Device[]?> GetDevices(int homeId) => await GetData<Models.Device[]>($"homes/{homeId}/devices");

    /// <summary>
    /// Returns the mobile devices connected to the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The connected mobile devices</returns>
    public async Task<Models.MobileDevice.Item[]?> GetMobileDevices(int homeId) => await GetData<Models.MobileDevice.Item[]>($"homes/{homeId}/mobileDevices");

    /// <summary>
    /// Returns the settings of a mobile device connected to the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <param name="mobileDeviceId">Id of the mobile device to query</param>
    /// <returns>The settings of the connected mobile device</returns>
    public async Task<Models.MobileDevice.Settings?> GetMobileDeviceSettings(int homeId, short mobileDeviceId) => await GetData<Models.MobileDevice.Settings>($"homes/{homeId}/mobileDevices/{mobileDeviceId}/settings");

    /// <summary>
    /// Returns the installations in the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The installations</returns>
    public async Task<Models.Installation[]?> GetInstallations(int homeId, short mobileDeviceId) => await GetData < Models.Installation[]>($"homes/{homeId}/installations");

    /// <summary>
    /// Returns the state of the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The state of the home</returns>
    public async Task<Models.HomeState?> GetHomeState(int homeId) => await GetData<Models.HomeState>($"homes/{homeId}/state");

    /// <summary>
    /// Returns the state of a zone in the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <param name="zoneId">Id of the zone to query</param>
    /// <returns>The state of the zone</returns>
    public async Task<Models.State?> GetZoneState(int homeId, short zoneId) => await GetData<Models.State>($"homes/{homeId}/state");

    /// <summary>
    /// Returns the summarized state of a zone in the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <param name="zoneId">Id of the zone to query</param>
    /// <returns>The summarized state of the zone</returns>
    public async Task<Models.ZoneSummary?> GetSummarizedZoneState(int homeId, short zoneId) => await GetData<Models.ZoneSummary>($"homes/{homeId}/zones/{zoneId}/overlay");

    /// <summary>
    /// Returns the current weather at the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The current weater at the home</returns>
    public async Task<Models.Weather?> GetWeather(int homeId) => await GetData<Models.Weather>($"homes/{homeId}/weather");

    /// <summary>
    /// Returns the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The home details</returns>
    public async Task<Models.House?> GetHome(int homeId) => await GetData<Models.House>($"homes/{homeId}");

    /// <summary>
    /// Returns the users with access to the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <returns>The users with access</returns>
    public async Task<Models.User[]?> GetUsers(int homeId) => await GetData<Models.User[]>($"homes/{homeId}/users");

    /// <summary>
    /// Returns the capabilities of a zone in the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <param name="zoneId">Id of the zone to query</param>
    /// <returns>The capabilities of the zone</returns>
    public async Task<Models.Capability?> GetZoneCapabilities(int homeId, int zoneId) => await GetData<Models.Capability>($"homes/{homeId}/zones/{zoneId}/capabilities");

    /// <summary>
    /// Returns the early start of a zone in the home with the provided Id from the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to query</param>
    /// <param name="zoneId">Id of the zone to query</param>
    /// <returns>The early start setting of the zone</returns>
    public async Task<Models.EarlyStart?> GetEarlyStart(int homeId, int zoneId) => await GetData<Models.EarlyStart>($"homes/{homeId}/zones/{zoneId}/earlyStart");

    /// <summary>
    /// Returns the temperature offset set for a specific device from the Tado API
    /// </summary>
    /// <param name="deviceId">Id of the device to query</param>
    /// <returns>The zone temperature offset in Celcius and Fahrenheit</returns>
    public async Task<Models.Temperature?> GetZoneTemperatureOffset(string deviceId) => await GetData<Models.Temperature>($"devices/{deviceId}/temperatureOffset");

    /// <summary>
    /// Returns the temperature offset set for a specific device from the Tado API
    /// </summary>
    /// <param name="device">The device to query</param>
    /// <returns>The zone temperature offset in Celcius and Fahrenheit</returns>
    public async Task<Models.Temperature?> GetZoneTemperatureOffset(Models.Device device) => await GetData<Models.Temperature>($"devices/{device.ShortSerialNo}/temperatureOffset");

    /// <summary>
    /// Returns the temperature offset set for a specific zone from the Tado API
    /// </summary>
    /// <param name="zone">The zone to query</param>
    /// <returns>The zone temperature offset in Celcius and Fahrenheit</returns>
    public async Task<Models.Temperature?> GetZoneTemperatureOffset(Models.Zone zone) => await GetData<Models.Temperature>($"devices/{zone.Devices?[0].ShortSerialNo}/temperatureOffset");

    #endregion

    #region Send Commands

    /// <summary>
    /// Internal method to send a command to the Tado API
    /// </summary>
    /// <param name="endPoint">The endpoint to be called</param>
    /// <param name="httpMethod">The HTTP method to use</param>
    /// <param name="expectedStatusCode">The expected response status. If omitted, HTTP 204 NO CONTENT is assumed.</param>
    /// <param name="body">Body of the message. Optional.</param>
    /// <returns>True if the call succeeded, false if it failed</returns>
    private async Task<bool> SendMessage(string endPoint, HttpMethod httpMethod, HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent, string body = "{}")
    {
        var token = await EnsureValidToken();

        var response = await HttpController.SendMessage(body, httpMethod, new Uri(TadoApiBaseUrl, endPoint), expectedStatusCode, token);
        return response;
    }

    /// <summary>
    /// Internal method to send a command to the Tado API
    /// </summary>
    /// <typeparam name="T">Model type to parse the response as</typeparam>
    /// <param name="endPoint">The endpoint to be called</param>
    /// <param name="httpMethod">The HTTP method to use</param>
    /// <param name="expectedStatusCode">The expected response status. If omitted, HTTP 204 NO CONTENT is assumed.</param>
    /// <returns>The typed data</returns>
    private async Task<T?> SendMessageReturnResponse<T>(string endPoint, string body, HttpMethod httpMethod, HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent)
    {
        var token = await EnsureValidToken();

        var response = await HttpController.SendMessageReturnResponse<T>(body, httpMethod, new Uri(TadoApiBaseUrl, endPoint), expectedStatusCode, token);
        return response;
    }

    /// <summary>
    /// Sets the specified zone to be in a "open window" state, if an open window is detected.
    /// Check if an open window is detected by Tado by looking at <see cref="Models.State.OpenWindowDetected"/>.
    /// </summary>
    /// <param name="homeId">Id of the home to set the "open window" state for</param>
    /// <param name="zoneId">Id of the zone to set the "open window" state for</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetOpenWindow(int homeId, int zoneId) => await SendMessage($"homes/{homeId}/zones/{zoneId}/state/openWindow/activate", HttpMethod.Post);

    /// <summary>
    /// Resets the specified zone back to a "closed window" state, if it is currently in a "open window" state
    /// </summary>
    /// <param name="homeId">Id of the home to reset the "open window" state for</param>
    /// <param name="zoneId">Id of the zone to reset the "open window" state for</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> ResetOpenWindow(int homeId, int zoneId) => await SendMessage($"homes/{homeId}/zones/{zoneId}/state/openWindow", HttpMethod.Delete);

    /// <summary>
    /// Sets the Home Presence mode manually, regardless of current geofence status of home devices.
    /// </summary>
    /// <param name="homeId">Id of the home to set the home presence for</param>
    /// <param name="presence">Presence to set for the home</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid enum value is used</exception>
    public async Task<bool> SetHomePresence(int homeId, Enums.HomePresence presence) => await SendMessage($"homes/{homeId}/presenceLock", HttpMethod.Put);


    /// <summary>
    /// Sets the EarlyStart mode of a zone in the home with the provided Id through the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to switch the heating off in</param>
    /// <param name="zoneId">Id of the zone to switch the heating off in</param>
    /// <param name="enabled">True to enable EarlyStart or False to disable it</param>
    /// <returns>The new EarlyStart mode of the zone</returns>
    public async Task<Models.EarlyStart?> SetEarlyStart(int homeId, int zoneId, bool enabled)
    {
        var earlyStart = new Models.EarlyStart
        {
            Enabled = enabled
        };
        var request = System.Text.Json.JsonSerializer.Serialize(earlyStart);
        var response = await SendMessageReturnResponse<Models.EarlyStart>($"homes/{homeId}/zones/{zoneId}/earlyStart", request, HttpMethod.Put, HttpStatusCode.OK);
        return response;
    }

    /// <summary>
    /// Shows Hi on the Tado device to identify it
    /// </summary>
    /// <param name="deviceId">Id / serial number of the Tado device</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SayHi(string deviceId) => await SendMessage($"devices/{deviceId}/identify", HttpMethod.Post, HttpStatusCode.OK);

    /// <summary>
    /// Turns the child lock on or off on the provided Tado device
    /// </summary>
    /// <param name="device">The Tado device to set the childlock for</param>
    /// <param name="enableChildLock">Boolean indicating if the childlock should be enabled (true) or disabled (false)</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetDeviceChildLock(Models.Device device, bool enableChildLock)
    {
        if (device.ShortSerialNo is null) return false;
        return await SetDeviceChildLock(device.ShortSerialNo, enableChildLock);
    }

    /// <summary>
    /// Turns the child lock on or off on the Tado device with the provided Id
    /// </summary>
    /// <param name="deviceId">Id of the Tado device to set the childlock for</param>
    /// <param name="enableChildLock">Boolean indicating if the childlock should be enabled (true) or disabled (false)</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetDeviceChildLock(string deviceId, bool enableChildLock)
    {
        var request = System.Text.Json.JsonSerializer.Serialize(new { childLockEnabled = enableChildLock });
        return await SendMessage($"devices/{deviceId}/childLock", HttpMethod.Put, HttpStatusCode.NoContent, request);
    }

    /// <summary>
    /// Sets the temperature offset in Celcius of a specific zone 
    /// </summary>
    /// <param name="deviceId">Id of the Tado device in the zone to set the offset for</param>
    /// <param name="temperature">Temperature in Celcius to set the offset to</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetZoneTemperatureOffsetCelcius(string deviceId, double temperature)
    {
        var request = System.Text.Json.JsonSerializer.Serialize(new { celsius = temperature });
        return await SendMessage($"devices/{deviceId}/temperatureOffset", HttpMethod.Put, HttpStatusCode.OK, request);
    }

    /// <summary>
    /// Sets the temperature in a zone in the home with the provided Id through the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to set the temperature of</param>
    /// <param name="zoneId">Id of the zone to set the temperature of</param>
    /// <param name="temperature">Temperature to set the zone to</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SetHeatingTemperatureCelsius(int homeId, int zoneId, double temperature)
    {
        return await SetHeatingTemperatureCelcius(homeId, zoneId, temperature, Enums.DurationModes.UntilNextManualChange);
    }

    /// <summary>
    /// Sets the temperature in a zone in the home with the provided Id through the Tado API for the duration as specified
    /// </summary>
    /// <param name="homeId">Id of the home to set the temperature of</param>
    /// <param name="zoneId">Id of the zone to set the temperature of</param>
    /// <param name="temperature">Temperature to set the zone to</param>
    /// <param name="durationMode">Defines the duration for which the heating will be switched to the provided temperature</param>
    /// <param name="timer">Only applicapble if for durationMode Timer has been chosen. In that case it allows providing for how long the duration should be.</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SetHeatingTemperatureCelcius(int homeId, int zoneId, double temperature, Enums.DurationModes durationMode, TimeSpan? timer = null)
    {
        return await SetTemperature(homeId, zoneId, temperature, null, Enums.DeviceTypes.Heating, durationMode, timer);
    }

    /// <summary>
    /// Sets the hot water temperature in the home with the provided Id through the Tado API for the duration as specified
    /// </summary>
    /// <param name="homeId">Id of the home to set the temperature of</param>
    /// <param name="temperatureCelcius">Temperature in Celcius to set the zone to</param>
    /// <param name="durationMode">Defines the duration for which the heating will be switched to the provided temperature</param>
    /// <param name="timer">Only applicapble if for durationMode Timer has been chosen. In that case it allows providing for how long the duration should be.</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SetHotWaterTemperatureCelcius(int homeId, double temperatureCelcius, Enums.DurationModes durationMode, TimeSpan? timer = null)
    {
        // Tado Hot Water is zone 0
        return await SetTemperature(homeId, 0, temperatureCelcius, null, Enums.DeviceTypes.HotWater, durationMode, timer);
    }

    /// <summary>
    /// Sets the hot water temperature in the home with the provided Id through the Tado API for the duration as specified
    /// </summary>
    /// <param name="homeId">Id of the home to set the temperature of</param>
    /// <param name="temperatureFahrenheit">Temperature in Fahrenheit to set the zone to</param>
    /// <param name="durationMode">Defines the duration for which the heating will be switched to the provided temperature</param>
    /// <param name="timer">Only applicapble if for durationMode Timer has been chosen. In that case it allows providing for how long the duration should be.</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SetHotWaterTemperatureFahrenheit(int homeId, double temperatureFahrenheit, Enums.DurationModes durationMode, TimeSpan? timer = null)
    {
        // Tado Hot Water is zone 0
        return await SetTemperature(homeId, 0, null, temperatureFahrenheit, Enums.DeviceTypes.HotWater, durationMode, timer);
    }

    /// <summary>
    /// Sets the temperature in a zone in the home with the provided Id through the Tado API for the duration as specified
    /// </summary>
    /// <param name="homeId">Id of the home to set the temperature of</param>
    /// <param name="zoneId">Id of the zone to set the temperature of</param>
    /// <param name="temperatureCelcius">Temperature in Celcius to set the zone to. Provide NULL for both temperatureCelcius and temperatureFahrenheit to switch the device off.</param>
    /// <param name="temperatureFahrenheit">Temperature in Fahrenheit to set the zone to. Provide NULL for both temperatureCelcius and temperatureFahrenheit to switch the device off.</param>
    /// <param name="durationMode">Defines the duration for which the heating will be switched to the provided temperature</param>
    /// <param name="timer">Only applicapble if for durationMode Timer has been chosen. In that case it allows providing for how long the duration should be.</param>
    /// <param name="deviceType">Type of Tado device to switch on</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SetTemperature(int homeId, int zoneId, double? temperatureCelcius, double? temperatureFahrenheit, Enums.DeviceTypes deviceType, Enums.DurationModes durationMode, TimeSpan? timer = null)
    {
        Helpers.EnumValidation.EnsureEnumWithinRange(deviceType);
        Helpers.EnumValidation.EnsureEnumWithinRange(durationMode);

        // If using Timer mode but not providing a timer duration, switch it to manual
        if (durationMode == Enums.DurationModes.Timer && timer == null)
        {
            durationMode = Enums.DurationModes.UntilNextManualChange;
        }

        var token = await EnsureValidToken();

        // Define the proper command for the provided duration mode
        var overlay = new Models.Overlay
        {
            Setting = new Models.Setting
            {
                DeviceType = deviceType
            },
            Termination = new Models.Termination
            {
                CurrentType = durationMode
            }
        };

        // If no temperature in Celcius and Fahrenheit has been provided, instruct to switch the device off, otherwise instruct to switch it on
        overlay.Setting.Power = !temperatureCelcius.HasValue && !temperatureFahrenheit.HasValue ? Enums.PowerStates.Off : Enums.PowerStates.On;

        // If the device is about to be switched on, provide the temperature it should be switched to
        if (overlay.Setting.Power == Enums.PowerStates.On)
        {
            overlay.Setting.Temperature = new Models.Temperature
            {
                Celsius = temperatureCelcius,
                Fahrenheit = temperatureFahrenheit
            };
        }

        if (durationMode == Enums.DurationModes.Timer && timer.HasValue)
        {
            overlay.Termination.DurationInSeconds = (int) timer.Value.TotalSeconds;
        }

        var request = System.Text.Json.JsonSerializer.Serialize(overlay);
        var response = await SendMessageReturnResponse<Models.ZoneSummary>($"homes/{homeId}/zones/{zoneId}/overlay", request, HttpMethod.Put, HttpStatusCode.OK);
        return response;
    }

    /// <summary>
    /// Sets the temperature in a zone in the home with the provided Id through the Tado API
    /// </summary>
    /// <param name="homeId">Id of the home to set the temperature of</param>
    /// <param name="zoneId">Id of the zone to set the temperature of</param>
    /// <param name="temperature">Temperature to set the zone to</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SetHeatingTemperatureFahrenheit(int homeId, int zoneId, double temperature)
    {
        return await SetHeatingTemperatureFahrenheit(homeId, zoneId, temperature, Enums.DurationModes.UntilNextManualChange);
    }

    /// <summary>
    /// Sets the temperature in a zone in the home with the provided Id through the Tado API for the duration as specified
    /// </summary>
    /// <param name="homeId">Id of the home to set the temperature of</param>
    /// <param name="zoneId">Id of the zone to set the temperature of</param>
    /// <param name="temperature">Temperature to set the zone to</param>
    /// <param name="durationMode">Defines the duration for which the heating will be switched to the provided temperature</param>
    /// <param name="timer">Only applicapble if for durationMode Timer has been chosen. In that case it allows providing for how long the duration should be.</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SetHeatingTemperatureFahrenheit(int homeId, int zoneId, double temperature, Enums.DurationModes durationMode, TimeSpan? timer = null)
    {
        return await SetTemperature(homeId, zoneId, null, temperature, Enums.DeviceTypes.Heating, durationMode, timer);
    }

    /// <summary>
    /// Switches the heating off in a zone in the home with the provided Id through the Tado API. Use SetTemperatureCelsius or SetTemperatureFahrenheit to switch the heating on again.
    /// </summary>
    /// <param name="homeId">Id of the home to switch the heating off in</param>
    /// <param name="zoneId">Id of the zone to switch the heating off in</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SwitchHeatingOff(int homeId, int zoneId)
    {
        return await SwitchHeatingOff(homeId, zoneId, Enums.DurationModes.UntilNextManualChange);
    }

    /// <summary>
    /// Switches the heating off in a zone in the home with the provided Id through the Tado API for the duration as specified
    /// </summary>
    /// <param name="homeId">Id of the home to switch the heating off in</param>
    /// <param name="zoneId">Id of the zone to switch the heating off in</param>
    /// <param name="durationMode">Defines the duration for which the temperature will remain switched off</param>
    /// <param name="timer">Only applicapble if for durationMode Timer has been chosen. In that case it allows providing for how long the duration should be.</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SwitchHeatingOff(int homeId, int zoneId, Enums.DurationModes durationMode, TimeSpan? timer = null)
    {
        return await SetTemperature(homeId, zoneId, null, null, Enums.DeviceTypes.Heating, durationMode, timer);
    }

    /// <summary>
    /// Switches the hot water off in the home with the provided Id through the Tado API for the duration as specified
    /// </summary>
    /// <param name="homeId">Id of the home to switch the heating off in</param>
    /// <param name="durationMode">Defines the duration for which the temperature will remain switched off</param>
    /// <param name="timer">Only applicapble if for durationMode Timer has been chosen. In that case it allows providing for how long the duration should be.</param>
    /// <returns>The summarized new state of the zone</returns>
    public async Task<Models.ZoneSummary?> SwitchHotWaterOff(int homeId, Enums.DurationModes durationMode, TimeSpan? timer = null)
    {
        return await SetTemperature(homeId, 0, null, null, Enums.DeviceTypes.HotWater, durationMode, timer);
    }

    /// <summary>
    /// Sets the temperature offset in Celcius of a specific zone 
    /// </summary>
    /// <param name="device">The Tado device in the zone to set the offset for</param>
    /// <param name="temperature">Temperature in Celcius to set the offset to</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetZoneTemperatureOffsetCelcius(Models.Device device, double temperature)
    {
        if (device.ShortSerialNo is null) return false;
        return await SetZoneTemperatureOffsetCelcius(device.ShortSerialNo, temperature);
    }

    /// <summary>
    /// Sets the temperature offset in Celcius of a specific room 
    /// </summary>
    /// <param name="zone">The Tado zone to set the offset for</param>
    /// <param name="temperature">Temperature in Celcius to set the offset to</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetZoneTemperatureOffsetCelcius(Models.Zone zone, double temperature)
    {
        if (zone == null)
        {
            throw new ArgumentNullException(nameof(zone));
        }
        if (zone.Devices is null || zone.Devices.Length == 0)
        {
            throw new ArgumentException("Provided zone has no devices registered to it", nameof(zone));
        }
        if (string.IsNullOrWhiteSpace(zone.Devices[0].ShortSerialNo))
        {
            throw new ArgumentException("The first device in the provided zone does not have a serial number assigned to it", nameof(zone));
        }

        return await SetZoneTemperatureOffsetCelcius(zone.Devices?[0].ShortSerialNo, temperature);
    }

    /// <summary>
    /// Sets the temperature offset in Fahrenheit of a specific zone 
    /// </summary>
    /// <param name="deviceId">Id of the Tado device in the zone to set the offset for</param>
    /// <param name="temperature">Temperature in Fahrenheit to set the offset to</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetZoneTemperatureOffsetFahrenheit(string deviceId, double temperature)
    {
        var request = System.Text.Json.JsonSerializer.Serialize(new { fahrenheit = temperature });
        return await SendMessage($"devices/{deviceId}/temperatureOffset", HttpMethod.Put, HttpStatusCode.OK, request);
    }

    /// <summary>
    /// Sets the temperature offset in Fahrenheit of a specific zone 
    /// </summary>
    /// <param name="device">The Tado device in the zone to set the offset for</param>
    /// <param name="temperature">Temperature in Fahrenheit to set the offset to</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetZoneTemperatureOffsetFahrenheit(Models.Device device, double temperature)
    {
        if (string.IsNullOrWhiteSpace(device.ShortSerialNo)) return false;
        return await SetZoneTemperatureOffsetFahrenheit(device.ShortSerialNo, temperature);
    }

    /// <summary>
    /// Sets the temperature offset in Fahrenheit of a specific room 
    /// </summary>
    /// <param name="zone">The Tado zone to set the offset for</param>
    /// <param name="temperature">Temperature in Fahrenheit to set the offset to</param>
    /// <returns>Boolean indicating if the request was successful</returns>
    public async Task<bool> SetZoneTemperatureOffsetFahrenheit(Models.Zone zone, double temperature)
    {
        if (zone == null)
        {
            throw new ArgumentNullException(nameof(zone));
        }
        if (zone.Devices is null || zone.Devices.Length == 0)
        {
            throw new ArgumentException("Provided zone has no devices registered to it", nameof(zone));
        }
        if (string.IsNullOrWhiteSpace(zone.Devices[0].ShortSerialNo))
        {
            throw new ArgumentException("The first device in the provided zone does not have a serial number assigned to it", nameof(zone));
        }

        return await SetZoneTemperatureOffsetFahrenheit(zone.Devices[0].ShortSerialNo, temperature);
    }

    #endregion
}