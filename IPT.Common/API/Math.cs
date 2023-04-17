using Rage;

namespace IPT.Common.API
{
    /// <summary>
    /// Additional math-related functions.
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// A value used for converting radians to degrees.
        /// </summary>
        public static readonly double Rad2Deg = 180.0 / System.Math.PI;

        /// <summary>
        /// A value used for converting degrees to radians.
        /// </summary>
        public static readonly double Deg2Rad = System.Math.PI / 180.0;

        /// <summary>
        /// Clamps a float between two (inclusive) values.
        /// </summary>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">This maximum value.</param>
        /// <returns>A float that exists between the min and max.</returns>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }

        /// <summary>
        /// Clamps an integer between two (inclusive) values.
        /// </summary>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>An integer that exists between the min and max.</returns>
        public static int Clamp(int value, int min, int max)
        {
            return System.Math.Min(max, System.Math.Max(min, value));
        }

        /// <summary>
        /// Applies a clamp to a float and also snaps to the nearest larger incremental value.
        /// </summary>
        /// <param name="value">The float to be snapped.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="increment">The snapping interval.</param>
        /// <returns>A float that has been snapped.</returns>
        public static float Snap(float value, float min, float max, float increment)
        {
            float cValue = Clamp(value, min, max);
            for (float i = min; i <= max; i += increment)
            {
                if (i >= cValue)
                {
                    return i;
                }
            }

            return cValue;
        }

        /// <summary>
        /// Applies clamp to an integer and also snaps to the nearest larger incremental value.
        /// </summary>
        /// <param name="value">The integer to be snapped.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="increment">The snappint interval.</param>
        /// <returns>An integer that has been snapped.</returns>
        public static int Snap(int value, int min, int max, int increment)
        {
            int cValue = Clamp(value, min, max);
            for (int i = min; i <= max; i += increment)
            {
                if (i >= cValue)
                {
                    return i;
                }
            }

            return cValue;
        }

        /// <summary>
        /// Calculate the angle in degrees from the front of the origin entity to the target entity.
        /// </summary>
        /// <param name="origin">The origin entity.</param>
        /// <param name="target">The target entity.</param>
        /// <param name="originHeading">The heading of the origin in degrees.</param>
        /// <returns>A float value representing the smallest angle in degrees between the origin and the target.</returns>
        public static double CalculateAngleOffset(Vector3 origin, Vector3 target, float originHeading)
        {
            double offset = originHeading - CalculateAngle(origin, target);
            if (offset < -180.0)
            {
                offset += 360.0;
            }
            else if (offset > 180.0)
            {
                offset -= 360.0;
            }

            return offset;
        }

        /// <summary>
        /// Calculates the cartesian distance between two entities in 2D space.
        /// </summary>
        /// <param name="origin">The origin entity.</param>
        /// <param name="target">The target entity.</param>
        /// <returns>A double value representing the smallest angle in degrees between the origin and the target.</returns>
        public static double CalculateCartesianDistance(Vector3 origin, Vector3 target)
        {
            var dX = target.X - origin.X;
            var dY = target.Y - origin.Y;
            return System.Math.Sqrt((dX * dX) + (dY * dY));
        }

        /// <summary>
        /// Calculates the heading angle in degrees from the origin point to the target point in 2D space.
        /// The angle is measured counterclockwise from the positive Y-axis such that:
        ///     North = 0
        ///     East = 270
        ///     South = 180
        ///     West = 90
        /// The positive-Y axis is used as zero because that is what Rage uses for entity headings.
        /// </summary>
        /// <param name="origin">The origin point.</param>
        /// <param name="target">The target point.</param>
        /// <returns>A float value representing the heading angle in degrees between the two points.</returns>
        public static double CalculateAngle(Vector3 origin, Vector3 target)
        {
            var radians = System.Math.Atan2(target.Y - origin.Y, target.X - origin.X);
            var degrees = ConvertRadiansToDegrees(radians) - 90.0;
            return degrees >= 0.0 ? degrees : degrees + 360.0;
        }

        /// <summary>
        /// Converts radians into degrees.
        /// </summary>
        /// <param name="radians">The initial value in radians.</param>
        /// <returns>A double value representing the radians in degrees.</returns>
        public static double ConvertRadiansToDegrees(double radians)
        {
            return radians * Rad2Deg;
        }

        /// <summary>
        /// Converts degrees into radians.
        /// </summary>
        /// <param name="degrees">The initial value in degrees.</param>
        /// <returns>A double value representing the degrees in radians.</returns>
        public static double ConvertDegreesToRadians(double degrees)
        {
            return degrees * Deg2Rad;
        }
    }
}
