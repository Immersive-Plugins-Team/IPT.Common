namespace IPT.Common.API
{
    /// <summary>
    /// Additional math-related functions.
    /// </summary>
    public static class Math
    {
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
    }
}
