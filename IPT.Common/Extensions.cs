namespace IPT.Common
{
    /// <summary>
    /// Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// An extension for setting the player availability based on status.
        /// </summary>
        /// <param name="status">The player status.</param>
        public static void SetPlayerAvailability(this PlayerStatus status)
        {
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
        }

        /// <summary>
        /// Gets a colorized notification string for the status.
        /// </summary>
        /// <param name="status">The player's status.</param>
        /// <returns>The color coded string.</returns>
        public static string ToColorString(this PlayerStatus status)
        {
            switch (status)
            {
                case PlayerStatus.Available:
                    return "~g~Available";
                case PlayerStatus.Busy:
                    return "~y~Busy";
                case PlayerStatus.Emergency:
                    return "~r~Emergency";
                case PlayerStatus.EnRoute:
                    return "~o~En Route";
                case PlayerStatus.Investigating:
                    return "~o~Investigating";
                case PlayerStatus.MealBreak:
                    return "~y~Meal Break";
                case PlayerStatus.OnPatrol:
                    return "~g~On Patrol";
                case PlayerStatus.OnScene:
                    return "~o~On Scene";
                case PlayerStatus.OutOfService:
                    return "~y~Out Of Service";
                case PlayerStatus.Pursuit:
                    return "~o~In Pursuit";
                case PlayerStatus.ReturnToStation:
                    return "~o~Return To Station";
                case PlayerStatus.TrafficStop:
                    return "~o~Traffic Stop";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// Gets a plain string version of the status.
        /// </summary>
        /// <param name="status">The player's status.</param>
        /// <returns>A plain string representation.</returns>
        public static string ToPlainString(this PlayerStatus status)
        {
            return status.ToColorString().Substring(3);
        }
    }
}
