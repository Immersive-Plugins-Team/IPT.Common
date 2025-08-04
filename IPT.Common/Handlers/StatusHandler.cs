using System;
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
    public sealed class StatusHandler
    {
        private static readonly Lazy<StatusHandler> lazyInstance = new Lazy<StatusHandler>(() => new StatusHandler());
        private PlayerStatus _status = PlayerStatus.Available;
        private string _callsign = "1-LINCOLN-18";
        private int _callsignSetterPriority = -1;

        private StatusHandler() { }

        public static StatusHandler Instance => lazyInstance.Value;
        public event Action<PlayerStatus> OnStatusChanged;
        public string Callsign => _callsign;
        public PlayerStatus Status => _status;

        /// <summary>
        /// Sets the player's callsign.
        /// </summary>
        /// <param name="playerCallsign">The new callsign.</param>
        /// <param name="priority">The priority of the caller, higher priority takes precedence.</param>
        internal void SetCallsign(string playerCallsign, int priority = 0)
        {
            if (priority > _callsignSetterPriority)
            {
                _callsign = playerCallsign;
                _callsignSetterPriority = priority;
            }
        }

        /// <summary>
        /// Sets the player's status.
        /// </summary>
        /// <param name="playerStatus">The new status.</param>
        /// <param name="sendNotification">Sends a notification when true.</param>
        /// <param name="handleAvailability">Update the availability based on the status..</param>
        internal void SetStatus(PlayerStatus playerStatus, bool sendNotification = true, bool handleAvailability = true)
        {
            if (_status == playerStatus) return;
            _status = playerStatus;
            if (_status == PlayerStatus.EnRoute)
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

            if (handleAvailability) _status.SetPlayerAvailability();
            if (sendNotification) Notifications.StatusNotification();
            OnStatusChanged?.Invoke(playerStatus);
        }
    }
}
