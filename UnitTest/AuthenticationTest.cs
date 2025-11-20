using System.Diagnostics;
namespace KoenZomers.Tado.Api;

/// <summary>
/// Integration test to validate the authenticating against the Tado API
/// Run all tests in the correct order. Make sure to register the device with the code between while running Test2!!
/// </summary>
[TestClass]
public class AuthenticationIntegrationTest : IntegrationTestBase
{
    [TestMethod]
    public async Task Run()
    {
        Assert.IsNotNull(Service, "Service not available.");
        Assert.IsNotNull(Configuration, "Configuration not available (appsettings.json).");
        Assert.IsNotNull(Configuration.TadoHomeId, "TadoHomeId must be set in the appsettings.json for this test to work.");
        
        // Step 1: Retrieve a new device code
        var deviceAuthorizationResponse = await Service.GetDeviceCodeAuthentication(TestContext.CancellationToken);
        Assert.IsNotNull(deviceAuthorizationResponse, "Failed to instantiate device authentication flow.");

        // Step2: Go to the tado URL and authenticate your device
        Debug.WriteLine("URL for authentication: {0}", deviceAuthorizationResponse.VerificationUriComplete);
        Console.WriteLine("URL for authentication: {0}", deviceAuthorizationResponse.VerificationUriComplete);
        TestContext.WriteLine("URL for authentication: {0}", deviceAuthorizationResponse.VerificationUriComplete);
        
        // Step3: Wait for the user to complete the authentication
        var token = await Service.WaitForDeviceCodeAuthenticationToComplete(deviceAuthorizationResponse, TestContext.CancellationToken);
        Assert.IsNotNull(token, "Failed to complete device authentication flow.");

        // Step4: Try to refresh the access token with the refresh token
        var newToken = await Service.GetAccessTokenWithRefreshToken(token.RefreshToken!, TestContext.CancellationToken);
        Assert.IsNotNull(newToken, "Failed to retrieve new access token with refresh token.");
        Assert.AreNotEqual(token.RefreshToken, newToken.RefreshToken);
        Assert.AreNotEqual(token.AccessToken, newToken.AccessToken);
        
        // Step5: Try to fetch device data with the access token and let the thermostat say 'Hi'. 
        Service.Authenticate(newToken);
        var devices = await Service.GetDevices(Configuration!.TadoHomeId!.Value);
        Assert.IsNotNull(devices, "Failed to retrieve devices.");
        Assert.IsNotEmpty(devices, "No devices found.");
        
        var thermostatDevices = devices.Where(x => x.DeviceType!.StartsWith("RU")).ToArray();
        var success = thermostatDevices.Length > 0;
        foreach (var device in thermostatDevices)
        {
            success &= await Service.SayHi(device.SerialNo!, TestContext.CancellationToken);
        }
        Assert.IsTrue(success, "Failed to retrieve information from Tado and say Hi to devices.");
    }

    public TestContext TestContext { get; set; }
}