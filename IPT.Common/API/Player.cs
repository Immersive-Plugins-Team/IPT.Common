using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// Helpful player functions.
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// Gets the angle between the player and the entity.
        /// </summary>
        /// <param name="entity">The target entity.</param>
        /// <returns>The angle in degrees.</returns>
        public static double GetVectorAngleTo(Entity entity)
        {
            return Math.GetVectorAngle(Game.LocalPlayer.Character, entity);
        }
    }
}
