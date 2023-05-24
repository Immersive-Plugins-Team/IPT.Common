using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// Math helper.
    /// </summary>
    public static class MathHelperExtensions
    {
        /// <summary>
        /// Calculates a forward vector.
        /// </summary>
        /// <param name="heading">The heading for the vector.</param>
        /// <returns>A forward vector.</returns>
        public static Vector3 GetForwardVector(float heading)
        {
            var headingRads = MathHelper.ConvertDegreesToRadians(90f - heading);
            float x = System.Convert.ToSingle(-System.Math.Cos(headingRads));
            float y = System.Convert.ToSingle(System.Math.Sin(headingRads));
            return new Vector3(x, y, 0f);
        }

        /// <summary>
        /// Gets the angle (0-180 degrees) between the source and target entities.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <returns>A double indicating the angle in degrees.</returns>
        public static double GetVectorAngle(Entity source, Entity target)
        {
            var targetVector = target.Position - source.Position;
            var dotProduct = Vector3.Dot(source.ForwardVector.ToNormalized(), targetVector.ToNormalized());
            return MathHelper.ConvertRadiansToDegrees(System.Math.Acos(dotProduct));
        }

        /// <summary>
        /// Gets the angle (0-180 degrees) between the source and target positions given the heading.
        /// </summary>
        /// <param name="sourcePosition">The source position.</param>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="heading">The in-game heading (where 0 degrees is due north).</param>
        /// <returns>The offset from 0 degrees (straight towards) between the source and target along a heading.</returns>
        public static double GetVectorAngle(Vector3 sourcePosition, Vector3 targetPosition, float heading)
        {
            var targetVector = targetPosition - sourcePosition;
            var forwardVector = GetForwardVector(heading);
            var dotProduct = Vector3.Dot(forwardVector.ToNormalized(), targetVector.ToNormalized());
            return MathHelper.ConvertRadiansToDegrees(System.Math.Acos(dotProduct));
        }

        /// <summary>
        /// Wraps an angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The wrapped angle.</returns>
        public static float WrapAngle(float angle)
        {
            angle %= 360f;
            if (angle > 180f)
            {
                angle -= 360f;
            }
            else if (angle < -180f)
            {
                angle += 360f;
            }

            return angle;
        }
    }
}
