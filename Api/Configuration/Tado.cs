namespace KoenZomers.Tado.Api.Configuration;

/// <summary>
/// Configuration for the Tado API
/// </summary>
public class Tado
{
    /// <summary>
    /// Base URL for the Tado API
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// The URL to use for authentication against the Tado API
    /// </summary>
    public string? AuthenticationUrl { get; set; }

    /// <summary>
    /// The URL to use for retrieving a device code for authentication against the Tado API
    /// </summary>
    public string? TokenUrl { get; set; }

    /// <summary>
    /// Default timeout for HTTP requests in seconds
    /// </summary>
    public short? DefaultApiTimeoutSeconds { get; set; }

    /// <summary>
    /// Default user agent for HTTP requests towards Tado. Will be appended with the assembly version number.
    /// </summary>
    public string? UserAgent { get; set; } = "KoenZomers.Tado.Api";

    /// <summary>
    /// The client ID to use when connecting to the Tado API
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Id of the home as registered with Tado
    /// </summary>
    public int? TadoHomeId { get; set; }

    ///// <summary>
    ///// Id of the zone as registered with Tado
    ///// </summary>
    //public static int ZoneId => int.Parse(ConfigurationManager.AppSettings["TadoZoneId"]);

    ///// <summary>
    ///// Id of the mobile device as registered with Tado
    ///// </summary>
    //public static int MobileDeviceId => int.Parse(ConfigurationManager.AppSettings["TadoMobileDeviceId"]);

    ///// <summary>
    ///// Id of the Tado device
    ///// </summary>
    //public static string DeviceId => ConfigurationManager.AppSettings["TadoDeviceId"];
}
