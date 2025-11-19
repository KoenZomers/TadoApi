namespace KoenZomers.Tado.Api.Enums;

/// <summary>
/// Defines the modes of the duration of changing a temperature
/// </summary>
public enum DurationModes : short
{
    /// <summary>
    /// Keep the setting until the next scheduled event starts
    /// </summary>
    UntilNextTimedEvent,

    /// <summary>
    /// Keep the setting for a specific duration
    /// </summary>
    Timer,

    /// <summary>
    /// Keep the setting until the user makes another change
    /// </summary>
    UntilNextManualChange
}