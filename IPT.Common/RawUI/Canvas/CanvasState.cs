using Rage;
using Rage.Native;

namespace IPT.Common.RawUI.Canvas
{
    /// <summary>
    /// Represents the state of the canvas.
    /// </summary>
    public abstract class CanvasState
    {
        private bool isControlsEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasState"/> class.
        /// </summary>
        /// <param name="canvas">The context canvas for the state.</param>
        public CanvasState(Canvas canvas)
        {
            this.Canvas = canvas;
        }

        /// <summary>
        /// Gets the context canvas for the state.
        /// </summary>
        public Canvas Canvas { get; }

        /// <summary>
        /// Executed when drawing.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        public virtual void Draw(Rage.Graphics g)
        {
            foreach (var item in this.Canvas.Items)
            {
                if (item.IsVisible)
                {
                    item.Draw(g);
                }
            }
        }

        /// <summary>
        /// Processes user input.
        /// </summary>
        public virtual void ProcessControls()
        {
            // do nothing
        }

        /// <summary>
        /// Enables or disables the player controls.
        /// </summary>
        /// <param name="isEnabled">Whether or not to enable or disable the controls.</param>
        protected void SetPlayerControls(bool isEnabled)
        {
            if (isEnabled != this.isControlsEnabled)
            {
                NativeFunction.Natives.x8D32347D6D4C40A2(Game.LocalPlayer, isEnabled, 0);
                this.isControlsEnabled = isEnabled;
            }
        }
    }
}
