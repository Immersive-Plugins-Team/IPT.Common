namespace IPT.Common.RawUI.States
{
    /// <summary>
    /// Represents the state of the canvas mouse.
    /// </summary>
    public abstract class MouseState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseState"/> class.
        /// </summary>
        /// <param name="canvas">The context canvas for the state.</param>
        public MouseState(Canvas canvas)
        {
            this.Canvas = canvas;
        }

        /// <summary>
        /// Gets the context canvas for the state.
        /// </summary>
        public Canvas Canvas { get; }

        /// <summary>
        /// Updates the canvas active and hovered widgets.
        /// </summary>
        public abstract void UpdateWidgets();
    }
}
