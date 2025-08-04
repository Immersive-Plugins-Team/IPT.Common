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
            PlayerStateManager.Instance.SetCallsign(playerCallsign, priority);
        }   

        internal static void SetStatus(PlayerStatus playerStatus, bool sendNotification = true, bool handleAvailability = true)
        {
            if (PlayerStateManager.Instance.Status == playerStatus) return;
            PlayerStateManager.Instance.SetStatus(playerStatus, sendNotification, handleAvailability);
            API.Events.FirePlayerStatusChange(playerStatus);
        }


        public static string GetCallsign()
        {
            return PlayerStateManager.Instance.Callsign;
        }

        public static PlayerStatus GetStatus()
        {
            return PlayerStateManager.Instance.Status;
        }
    }
}
