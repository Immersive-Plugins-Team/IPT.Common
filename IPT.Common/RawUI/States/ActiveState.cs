using System.Linq;
using IPT.Common.API;
using Rage;
using Rage.Native;

namespace IPT.Common.RawUI.States
{
    /// <summary>
    /// Represents the canvas in an active state.
    /// </summary>
    public class ActiveState : CanvasState
    {
        private bool doPause = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveState"/> class.
        /// </summary>
        /// <param name="canvas">The context for the state.</param>
        /// <param name="doPause">Whether or not to pause when activating the canvas.</param>
        public ActiveState(Canvas canvas, bool doPause)
            : base(canvas)
        {
            this.doPause = doPause;
            this.MoveCursorToTopWidget();
            this.Canvas.Cursor.IsVisible = true;
            this.Canvas.Cursor.SetCursorType(CursorType.Default);
            if (!doPause)
            {
                this.SetPlayerControls(false);
            }
            else
            {
                Game.IsPaused = true;
            }
        }

        /// <inheritdoc/>
        public override void Draw(Graphics g)
        {
            if (!this.Canvas.IsGamePaused || this.doPause)
            {
                base.Draw(g);
                this.Canvas.Cursor.Draw(g);
            }
        }

        /// <inheritdoc/>
        public override void ProcessControls()
        {
            if (Game.Console.IsOpen || (this.Canvas.IsGamePaused && !this.doPause))
            {
                return;
            }
            else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CellphoneCancel))
            {
                this.SetPlayerControls(true);
                this.Canvas.SetCanvasState(new InactiveState(this.Canvas));
            }
            else
            {
                this.Canvas.Cursor.UpdateStatus();
            }
        }

        private void MoveCursorToTopWidget()
        {
            var topWidget = this.Canvas.Items.LastOrDefault();
            if (topWidget != null)
            {
                float x = topWidget.Bounds.X / this.Canvas.Resolution.Width;
                float y = topWidget.Bounds.Y / this.Canvas.Resolution.Height;
                NativeFunction.Natives.xFC695459D4D0E219(x, y);
            }
        }
    }
}
