using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KoenZomers.Tado.Api.Extensions;

/// <summary>
/// Tado Service Instantiation for the framework
/// </summary>
public static class PowerManagerService
{
    /// <summary>
    /// Instantiates the PowerManager Framework components
    /// </summary>
    /// <param name="basePath">Base path where the application files reside</param>
    public static Configuration.Tado AddTadoServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        Console.WriteLine("- Adding configuration");
        var serviceProvider = services.BuildServiceProvider();
        var configBuilder = serviceProvider.GetRequiredService<IConfiguration>();

        // Retrieve the PowerManager configuration section from the config file
        var configSection = configBuilder.GetSection("Tado");
        var config = configSection.Get<Configuration.Tado>() ?? throw new InvalidOperationException("Failed to create configuration");

        // Add the services
        services.Configure<Configuration.Tado>(configSection)
                .AddTadoControllers()
                .AddTadoHttpClients(config);

        return config;
    }

    /// <summary>
    /// Adds named HttpClient instances
    /// </summary>
    public static IServiceCollection AddTadoHttpClients(this IServiceCollection services, Configuration.Tado configuration)
    {
        // Define the User Agent to use in the HttpClient
        var userAgent = $"{configuration.UserAgent}/{System.Reflection.Assembly.GetExecutingAssembly()?.GetName().Version?.ToString(4)}";

        services.AddHttpClient(typeof(Controllers.Http).Name, client => { client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent); });

        return services;
    }

    /// <summary>
    /// Adds the controllers to the Tado framework
    /// </summary>
    public static IServiceCollection AddTadoControllers(this IServiceCollection services)
    {
        Console.WriteLine("- Adding controllers");

        services.AddSingleton<Controllers.Http>();
        services.AddSingleton<Controllers.Tado>();

        return services;
    }
}