using System.Runtime.CompilerServices;
using IPT.Common.API;
using Rage;

[assembly: InternalsVisibleTo("CalloutInterface")]
[assembly: InternalsVisibleTo("GrammarPolice")]

namespace IPT.Common.Handlers
{
    /// <summary>
    /// Used for managing player status.
    /// </summary>
    public static class PlayerHandler
    {
        private static PlayerStatus status = PlayerStatus.OutOfService;
        private static string callsign = "1-LINCOLN-18";

        /// <summary>
        /// Gets the player's callsign.
        /// </summary>
        /// <returns>A string representing the player's callsign.</returns>
        public static string GetCallsign()
        {
            return callsign;
        }

        /// <summary>
        /// Gets the current player status.
        /// </summary>
        /// <returns>The player's status.</returns>
        public static PlayerStatus GetStatus()
        {
            return status;
        }

        /// <summary>
        /// Sets the player's callsign.
        /// </summary>
        /// <param name="playerCallsign">The new callsign.</param>
        internal static void SetCallsign(string playerCallsign)
        {
            callsign = playerCallsign;
        }

        /// <summary>
        /// Sets the player's status.
        /// </summary>
        /// <param name="playerStatus">The new status.</param>
        /// <param name="sendNotification">Sends a notification when true.</param>
        /// <param name="handleAvailability">Update the availability based on the status..</param>
        internal static void SetStatus(PlayerStatus playerStatus, bool sendNotification = true, bool handleAvailability = true)
        {
            if (status == playerStatus)
            {
                return;
            }

            status = playerStatus;
            if (status == PlayerStatus.EnRoute)
            {
                var callout = LSPD_First_Response.Mod.API.Functions.GetCurrentCallout();
                if (callout == null)
                {
                    Logging.Warning("player attempted to go en route without a callout");
                    Game.DisplaySubtitle("There is no active or pending callout.");
                    return;
                }

                if (LSPD_First_Response.Mod.API.Functions.GetCalloutAcceptanceState(callout) == LSPD_First_Response.Mod.Callouts.CalloutAcceptanceState.Pending)
                {
                    try
                    {
                        LSPD_First_Response.Mod.API.Functions.AcceptPendingCallout(callout);
                    }
                    catch (System.Exception ex)
                    {
                        Logging.Error("could not accept callout", ex);
                    }
                }
            }

            if (handleAvailability)
            {
                playerStatus.SetPlayerAvailability();
            }

            if (sendNotification)
            {
                Notifications.StatusNotification();
            }

            API.Events.FirePlayerStatusChange(playerStatus);
        }
    }
}
