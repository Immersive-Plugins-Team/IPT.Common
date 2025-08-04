using IPT.Common.Handlers;
using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// Notification commands for a consistent style.
    /// </summary>
    public static class Notifications
    {
        /// <summary>
        /// Sends a notification that looks like a message from the dispatcher.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void DispatchMessage(string message)
        {
            Game.DisplayNotification($"~b~Dispatch: ~w~{message}");
        }

        /// <summary>
        /// Sends a dispach related notification.
        /// </summary>
        /// <param name="subtitle">The category of notification.</param>
        /// <param name="message">The message.</param>
        public static void DispatchNotification(string subtitle, string message)
        {
            OfficialNotification("DISPATCH", subtitle, message);
        }

        /// <summary>
        /// Sends an official looking notification.
        /// </summary>
        /// <param name="title">The title of the notification.</param>
        /// <param name="subtitle">The substitle of the notification.</param>
        /// <param name="message">The message of the notification.</param>
        public static void OfficialNotification(string title, string subtitle, string message)
        {
            Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", title, $"~b~{subtitle}", message);
        }

        /// <summary>
        /// Sends the status notification.
        /// </summary>
        public static void StatusNotification()
        {
            string message = $"~w~Callsign: ~g~{PlayerStateManager.Instance.Callsign}~n~~w~Status: {PlayerStateManager.Instance.Status.ToColorString()}";
            DispatchNotification("~b~Officer Status", message);
        }
    }
}
