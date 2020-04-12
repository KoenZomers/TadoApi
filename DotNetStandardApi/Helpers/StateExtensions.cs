using KoenZomers.Tado.Api.Entities;

namespace KoenZomers.Tado.Api.Helpers
{
    public static class StateExtensions
    {
        public static bool IsOpenWindowDetected(this State zoneState)
            => zoneState.OpenWindowDetected == true;
    }
}
