namespace IPT.Common.API
{
    /// <summary>
    /// Math helper.
    /// </summary>
    public static class MathHelperExtensions
    {
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
