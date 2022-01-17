namespace IPT.Common.API
{
    /// <summary>
    /// Common helper functions.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Checks to see if the game is paused.  Caution: This uses a NATIVE.
        /// </summary>
        /// <returns>True if the game is paused, otherwise false.</returns>
        public static bool IsGamePaused()
        {
            return Rage.Game.IsPaused || Rage.Native.NativeFunction.Natives.IS_PAUSE_MENU_ACTIVE<bool>();
        }
    }
}
