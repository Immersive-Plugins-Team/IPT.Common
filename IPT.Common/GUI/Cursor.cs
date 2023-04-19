using System.Drawing;
using IPT.Common.API;
using Rage;
using Rage.Native;

namespace IPT.Common.GUI
{
    /// <summary>
    /// A class for displaying and monitoring an on-screen cursor for editing a base frame.
    /// </summary>
    public class Cursor
    {
        /// <summary>
        /// Gets the location of the cursor on the canvas.
        /// </summary>
        public Point Position { get; private set; } = new Point(0, 0);

        /// <summary>
        /// Gets a value indicating whether or not the mouse is currently down.
        /// </summary>
        public MouseStatus MouseStatus { get; private set; } = MouseStatus.Up;

        /// <summary>
        /// Gets the current value of the scroll wheel.
        /// </summary>
        public ScrollWheelStatus ScrollWheelStatus { get; private set; } = ScrollWheelStatus.None;

        /// <summary>
        /// Draws the cursor.
        /// </summary>
        /// <param name="g">The Rage.Graphics object to draw against.</param>
        public virtual void Draw(Rage.Graphics g)
        {
            var xScale = Game.Resolution.Width / Constants.CanvasWidth;
            var yScale = Game.Resolution.Height / Constants.CanvasHeight;
            g.DrawFilledCircle(new Vector2(this.Position.X * xScale, this.Position.Y * yScale), 10f, Color.Green);
        }

        /// <summary>
        /// Updates the cursor's location, click status, and scroll wheel status.
        /// </summary>
        public void Update()
        {
            var x = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorX);
            var y = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorY);
            this.Position = new Point((int)System.Math.Round(x * Constants.CanvasWidth), (int)System.Math.Round(y * Constants.CanvasHeight));

            if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.Attack))
            {
                this.MouseStatus = MouseStatus.Down;
            }
            else
            {
                this.MouseStatus = MouseStatus.Up;
            }

            if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.WeaponWheelNext))
            {
                this.ScrollWheelStatus = ScrollWheelStatus.Up;
            }
            else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.WeaponWheelPrev))
            {
                this.ScrollWheelStatus = ScrollWheelStatus.Down;
            }
            else
            {
                this.ScrollWheelStatus = ScrollWheelStatus.None;
            }
        }
    }
}
