using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// A helpful wrapper around the Rage.Player class.
    /// </summary>
    public class PlayerPed : Ped
    {
        /// <summary>
        /// Gets and instance of the player ped from the player's character.
        /// </summary>
        /// <returns>A PlayerPed object.</returns>
        public static PlayerPed GetCharacter()
        {
            return (PlayerPed)Game.LocalPlayer.Character;
        }

        /// <summary>
        /// Gets the cartesian (inverted y-axis) angle between the player and the entity where due north is zero degrees.
        /// </summary>
        /// <param name="entity">The target entity.</param>
        /// <param name="frontOffset">The amount to offset from the front of the player when calculating angles.</param>
        /// <returns>The angle in degrees.</returns>
        public double GetAngleTo(Entity entity, float frontOffset = 0f)
        {
            return Math.CalculateAngle(this.GetOffsetPositionFront(frontOffset), entity.Position);
        }

        /// <summary>
        /// Gets the angle offset between the player and the entity.
        /// </summary>
        /// <param name="entity">The target entity.</param>
        /// <param name="frontOffset">The amount to offset from the front of the player when calculating angles.</param>
        /// <returns>The angle offset in degrees where positive values are counter-clockwise.</returns>
        public double GetAngleOffset(Entity entity, float frontOffset = 0f)
        {
            return Math.CalculateAngleOffset(this.GetOffsetPositionFront(frontOffset), entity.Position, this.Heading);
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
        public Ped GetNearestPed(float maxDistance, float maxAngle = 180f, float frontOffset = 0f, bool includePolice = true, bool includeDead = true)
        {
            var peds = World.GetAllPeds();
            Ped nearestPed = null;
            float minDistance = float.MaxValue;

            foreach (var ped in peds)
            {
                if (ped.IsPlayer || (!includePolice && ped.RelationshipGroup == "COP") || (!includeDead && !ped.IsAlive))
                {
                    continue;
                }

                var distance = this.DistanceTo2D(ped);
                if (distance <= maxDistance)
                {
                    var angle = System.Math.Abs(this.GetAngleOffset(ped, frontOffset));
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
