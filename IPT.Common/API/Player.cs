using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// Helpful player functions.
    /// </summary>
    public static class Player
    {
        /// <summary>
        /// Gets the cartesian (inverted y-axis) angle between the player and the entity where due north is zero degrees.
        /// </summary>
        /// <param name="entity">The target entity.</param>
        /// <param name="frontOffset">The amount to offset from the front of the player when calculating angles.</param>
        /// <returns>The angle in degrees.</returns>
        public static double GetAngleTo(Entity entity, float frontOffset = 0f)
        {
            return Math.CalculateAngle(Game.LocalPlayer.Character.GetOffsetPositionFront(frontOffset), entity.Position);
        }

        /// <summary>
        /// Gets the angle offset between the player and the entity.
        /// </summary>
        /// <param name="entity">The target entity.</param>
        /// <param name="frontOffset">The amount to offset from the front of the player when calculating angles.</param>
        /// <returns>The angle offset in degrees where positive values are counter-clockwise.</returns>
        public static double GetAngleOffset(Entity entity, float frontOffset = 0f)
        {
            var player = Game.LocalPlayer.Character;
            return Math.CalculateAngleOffset(player.GetOffsetPositionFront(frontOffset), entity.Position, player.Heading);
        }

        /// <summary>
        /// Gets the nearest Ped to the player within the maximum distance and angle.
        /// </summary>
        /// <param name="maxDistance">The maximum distance to search.</param>
        /// <param name="maxAngle">The maximum angle in degrees from the player's front (0 = directly in front, 180 = directly behind).</param>
        /// <param name="frontOffset">The amount to offset from the front of the player when calculating angles.</param>
        /// <param name="includePolice">Indicate whether or not to include police in the search.</param>
        /// <param name="includeDead">Indicate whether or not to include dead peds in the search.</param>
        /// <returns>The nearest ped if one is nearby otherwise null.</returns>
        public static Ped GetNearestPed(float maxDistance, float maxAngle = 180f, float frontOffset = 0f, bool includePolice = true, bool includeDead = true)
        {
            var peds = World.GetAllPeds();
            var player = Game.LocalPlayer.Character;
            Ped nearestPed = null;
            float minDistance = float.MaxValue;

            foreach (var ped in peds)
            {
                if (!ped || ped.IsPlayer || (!includePolice && ped.RelationshipGroup == "COP") || (!includeDead && !ped.IsAlive))
                {
                    continue;
                }

                var distance = player.DistanceTo2D(ped);
                if (distance <= maxDistance && distance > minDistance)
                {
                    if (System.Math.Abs(GetAngleOffset(ped, frontOffset)) <= maxAngle)
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
