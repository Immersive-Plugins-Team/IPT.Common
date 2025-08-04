using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CalloutInterface")]
[assembly: InternalsVisibleTo("GrammarPolice")]

namespace IPT.Common.Handlers
{
    /// <summary>
    /// Legacy adapter for the StatusHandler singleton. Maintained for backwards compatibility.
    /// </summary>
    [Obsolete("This static class is deprecated. Please use the singleton via StatusHandler.Instance instead.")]
    public static class PlayerHandler
    {
        internal static void SetCallsign(string playerCallsign, int priority = 0)
        {
            StatusHandler.Instance.SetCallsign(playerCallsign, priority);
        }   

        internal static void SetStatus(PlayerStatus playerStatus, bool sendNotification = true, bool handleAvailability = true)
        {
            if (StatusHandler.Instance.Status == playerStatus) return;
            StatusHandler.Instance.SetStatus(playerStatus);
            API.Events.FirePlayerStatusChange(playerStatus);
        }


        public static string GetCallsign()
        {
            return StatusHandler.Instance.Callsign;
        }

        public static PlayerStatus GetStatus()
        {
            return StatusHandler.Instance.Status;
        }
    }
}
