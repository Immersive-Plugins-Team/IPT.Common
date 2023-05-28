using System;
using Rage;
using Rage.Native;

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

        /// <summary>
        /// Loads a texture from disk.
        /// </summary>
        /// <param name="filename">The filename of the texture.</param>
        /// <returns>A Rage texture object.</returns>
        public static Texture LoadTexture(string filename)
        {
            try
            {
                return Game.CreateTextureFromFile(filename);
            }
            catch (Exception ex)
            {
                API.Logging.Error("could not load texture", ex);
            }

            return null;
        }

        /// <summary>
        /// Safely gets the current weather.
        /// </summary>
        /// <returns>The current weather type.</returns>
        public static EWeatherType GetWeatherType()
        {
            int weatherType = NativeFunction.Natives.GET_PREV_WEATHER_TYPE_HASH_NAME<int>();
            if (Enum.IsDefined(typeof(EWeatherType), weatherType))
            {
                return (EWeatherType)weatherType;
            }

            return EWeatherType.Neutral;
        }
    }
}
