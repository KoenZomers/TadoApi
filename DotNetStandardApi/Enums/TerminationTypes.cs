namespace KoenZomers.Tado.Api.Enums
{
    /// <summary>
    /// Defines the types to which a Tado device can be set that define the end of the current state of the device
    /// </summary>
    public enum TerminationTypes2 : short
    {
        /// <summary>
        /// The state will not change unless the device gets explicit instructions to do so
        /// </summary>
        Manual,

        /// <summary>
        /// The state of the device will change at the next scheduled event for the device
        /// </summary>
        NextScheduledEvent,

        /// <summary>
        /// The state of the device will change after a preset amount of time has elapsed
        /// </summary>
        Timer
    }
}
