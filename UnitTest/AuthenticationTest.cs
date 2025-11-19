using System.Diagnostics;

namespace KoenZomers.Tado.UnitTest;

/// <summary>
/// Unit Tests to validate authenticating against the Tado API
/// </summary>
[TestClass]
public class AuthenticationTest : BaseTest
{
    /// <summary>
    /// Test being able to retrieve an authorization URL
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task GetDeviceCodeAuthenticationTest()
    {
        if (Service is null) Assert.Fail("Service not available");

        var authenticationRequest = await Service.GetDeviceCodeAuthentication(CancellationToken.None);

        Assert.IsNotNull(authenticationRequest, "Failed to instantiate device authentication flow");
    }

    /// <summary>
    /// Test requesting a device authorization code and waiting for it to be completed
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task WaitForDeviceCodeAuthenticationToCompleteTest()
    {
        if (Service is null) Assert.Fail("Service not available");

        var authenticationRequest = await Service.GetDeviceCodeAuthentication(CancellationToken.None);

        Assert.IsNotNull(authenticationRequest, "Failed to instantiate device authentication flow");

        Debug.WriteLine($"URL for authentication: {authenticationRequest.VerificationUriComplete}");

        var tokenResponse = await Service.WaitForDeviceCodeAuthenticationToComplete(authenticationRequest, CancellationToken.None);

        Assert.IsNotNull(tokenResponse, "Failed to complete device authentication flow");

        Debug.WriteLine($"Access token: {tokenResponse.AccessToken}");
        Debug.WriteLine($"Refresh token: {tokenResponse.RefreshToken}");
    }

    /// <summary>
    /// Test requesting an access token with a refresh token
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task GetAccessTokenWithRefreshTokenTest()
    {
        if (Service is null) Assert.Fail("Service not available");

        var tokenResponse = await Service.GetAccessTokenWithRefreshToken("xxx", CancellationToken.None);

        Assert.IsNotNull(tokenResponse, "Failed to retrieve access token with refresh token");
    }

    /// <summary>
    /// Test requesting the Me endpoint from Tado
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task GetMeTest()
    {
        if (Service is null) Assert.Fail("Service not available");

        var authenticationRequest = await Service.GetDeviceCodeAuthentication(CancellationToken.None);

        Assert.IsNotNull(authenticationRequest, "Failed to instantiate device authentication flow");

        Debug.WriteLine($"URL for authentication: {authenticationRequest.VerificationUriComplete}");

        var tokenResponse = await Service.WaitForDeviceCodeAuthenticationToComplete(authenticationRequest, CancellationToken.None);

        Assert.IsNotNull(tokenResponse, "Failed to complete device authentication flow");

        Service.Authenticate(tokenResponse);

        var devices = await Service.GetDevices(Configuration.TadoHomeId.Value);

        var me = await Service.SayHi("xxxx");

        Assert.IsNotNull(me, "Failed to retrieve information from Tado");
    }
}