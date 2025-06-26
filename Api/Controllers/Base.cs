using Microsoft.Extensions.Logging;

namespace KoenZomers.Tado.Api.Controllers;

/// <summary>
/// Base class for all controllers
/// </summary>
/// <param name="loggerFactory">Instance through which to allow logging</param>
public abstract class Base(ILoggerFactory loggerFactory)
{
    /// <summary>
    /// Instance through which to perform logging
    /// </summary>
    protected ILogger Logger => loggerFactory.CreateLogger(GetType());
}
