using System.Runtime.CompilerServices;
using IPT.Common.API;
using static IPT.Common.Enums;

[assembly: InternalsVisibleTo("CalloutInterface")]
[assembly: InternalsVisibleTo("GrammarPolice")]

namespace IPT.Common.Handlers
{
    /// <summary>
    /// Used for managing player status.
    /// </summary>
    public static class PlayerStatusHandler
    {
        private static PlayerStatus status = PlayerStatus.OutOfService;

        /// <summary>
        /// Gets the current player status.
        /// </summary>
        /// <returns>The player's status.</returns>
        public static PlayerStatus GetPlayerStatus()
        {
            return status;
        }

        /// <summary>
        /// Sets the player's status.
        /// </summary>
        /// <param name="playerStatus">The new status.</param>
        internal static void SetPlayerStatus(PlayerStatus playerStatus)
        {
            if (status == playerStatus)
            {
                return;
            }

            status = playerStatus;
            if (status == PlayerStatus.EnRoute)
            {
                var callout = LSPD_First_Response.Mod.API.Functions.GetCurrentCallout();
                if (callout != null && LSPD_First_Response.Mod.API.Functions.GetCalloutAcceptanceState(callout) == LSPD_First_Response.Mod.Callouts.CalloutAcceptanceState.Pending)
                {
                    try
                    {
                        LSPD_First_Response.Mod.API.Functions.AcceptPendingCallout(callout);
                    }
                    catch (System.Exception ex)
                    {
                        Logging.Error("could not accept callout", ex);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            switch (status)
            {
                case PlayerStatus.Available:
                case PlayerStatus.OnPatrol:
                    LSPD_First_Response.Mod.API.Functions.SetPlayerAvailableForCalls(true);
                    break;

                case PlayerStatus.Busy:
                case PlayerStatus.Emergency:
                case PlayerStatus.EnRoute:
                case PlayerStatus.Investigating:
                case PlayerStatus.MealBreak:
                case PlayerStatus.OnScene:
                case PlayerStatus.OutOfService:
                case PlayerStatus.Pursuit:
                case PlayerStatus.ReturnToStation:
                case PlayerStatus.TrafficStop:
                    LSPD_First_Response.Mod.API.Functions.SetPlayerAvailableForCalls(false);
                    break;
            }

            API.Events.FirePlayerStatusChange(playerStatus);
        }
    }
}
