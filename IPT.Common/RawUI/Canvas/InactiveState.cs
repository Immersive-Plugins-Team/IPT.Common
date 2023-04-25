using System.Linq;
using Rage;

namespace IPT.Common.RawUI.Canvas
{
    /// <summary>
    /// Represents the canvas in an inactive state.
    /// </summary>
    public class InactiveState : CanvasState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InactiveState"/> class.
        /// </summary>
        /// <param name="canvas">The context for the state.</param>
        public InactiveState(Canvas canvas)
            : base(canvas)
        {
            Game.IsPaused = false;
            this.SetPlayerControls(true);
            this.Canvas.Cursor.IsVisible = false;
            this.ReleaseWidgets();
        }

        private void ReleaseWidgets()
        {
            foreach (var widget in this.Canvas.Items.OfType<IWidget>())
            {
                widget.IsHovered = false;
                widget.StopDrag();
            }
        }
    }
}
