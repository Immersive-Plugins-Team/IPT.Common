using System.Drawing;

namespace IPT.Common
{
    /// <summary>
    /// A list of project wide constant values.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The width of the canvas used for GUI elements.
        /// </summary>
        public const float CanvasWidth = 1920f;

        /// <summary>
        /// The height of the canvas used for GUI elements.
        /// </summary>
        public const float CanvasHeight = 1080f;

        /// <summary>
        /// The duration in milliseconds of a long click.
        /// </summary>
        public const long LongClick = 200;

        /// <summary>
        /// The minimum allowable scaling factor (as a percentage).
        /// </summary>
        public const float MinScale = 0.1f;

        /// <summary>
        /// The maximum allowable scaling factor (as a percentage).
        /// </summary>
        public const float MaxScale = 2f;

        /// <summary>
        /// How much to rescale when resizing widgets with the scrollwheel.
        /// </summary>
        public const float RescaleIncrement = 0.01f;

        /// <summary>
        /// The color to use when dragging graphical elements.
        /// </summary>
        public static readonly Color DraggingColor = Color.FromArgb(128, Color.Green);

        /// <summary>
        /// The color to use when hovering over graphical elements.
        /// </summary>
        public static readonly Color HoverColor = Color.FromArgb(128, Color.Yellow);
    }
}
