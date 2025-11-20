using KoenZomers.Tado.Api.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KoenZomers.Tado.Api;

/// <summary>
/// Base functionality shared by all Unit Tests
/// </summary>
public abstract class IntegrationTestBase
{
    /// <summary>
    /// The service provider for the application
    /// </summary>
    protected readonly IServiceProvider? ServiceProvider;

    /// <summary>
    /// Access to the service instance
    /// </summary>
    protected readonly Api.Controllers.Tado? Service;

    /// <summary>
    /// Access to the service configuration
    /// </summary>
    protected readonly Api.Configuration.Tado? Configuration;

    /// <summary>
    /// Instantiate the Unit Test by creating the service provider and retrieving the service instance to be tested
    /// </summary>
    protected IntegrationTestBase()
    {
        var services = new ServiceCollection();

        // Ensure the configuration is present
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
                .Build();
        services.AddSingleton<IConfiguration>(config);

        // Add the complete service stack
        Configuration = services.AddTadoServices();

        ServiceProvider = services.BuildServiceProvider();

        // Retrieve the requested service type to be tested
        Service = ServiceProvider.GetService<Api.Controllers.Tado>();

        // Ensure the service is present
        if (Service == null)
        {
            throw new InvalidOperationException($"Failed to create service");
        }
    }
}