using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// Helpful functions for the player.
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// Gets the nearest Ped to the player within the maximum distance and angle.
        /// </summary>
        /// <param name="maxDistance">The maximum distance to search.</param>
        /// <param name="maxAngle">The maximum angle in degrees from the player's front (0 = directly in front, 180 = directly behind).</param>
        /// <param name="includePolice">Indicate whether or not to include police in the search.</param>
        /// <returns>The nearest ped if one is nearby otherwise null.</returns>
        public static Ped GetNearestPed(float maxDistance, float maxAngle = 180f, bool includePolice = true)
        {
            var player = Game.LocalPlayer.Character;
            var peds = World.GetAllPeds();
            Ped nearestPed = null;
            float minDistance = float.MaxValue;

            foreach (var ped in peds)
            {
                if (ped.IsPlayer || (!includePolice && ped.RelationshipGroup == "COP"))
                {
                    continue;
                }

                var distance = player.DistanceTo2D(ped);
                if (distance <= maxDistance)
                {
                    var angle = Math.CalculateAngleToTarget(player, ped);
                    if (angle <= maxAngle && distance < minDistance)
                    {
                        nearestPed = ped;
                        minDistance = distance;
                    }
                }
            }

            return nearestPed;
        }
    }
}
