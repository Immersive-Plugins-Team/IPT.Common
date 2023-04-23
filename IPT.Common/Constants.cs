﻿using System.Drawing;

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
        public const long LongClick = 500;

        /// <summary>
        /// The minimum allowable scaling factor (as a percentage).
        /// </summary>
        public const int MinScale = 20;

        /// <summary>
        /// The maximum allowable scaling factor (as a percentage).
        /// </summary>
        public const int MaxScale = 200;

        /// <summary>
        /// The color to use when highlighting graphical elements.
        /// </summary>
        public static readonly Color HighlightColor = Color.Yellow;
    }
}
