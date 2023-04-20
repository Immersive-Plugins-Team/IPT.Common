using System.Drawing;
using IPT.Common.API;
using Rage;
using Rage.Native;

namespace IPT.Common.GUI
{
    /// <summary>
    /// A class for displaying and monitoring an on-screen cursor for editing a base frame.
    /// </summary>
    public class Cursor : TextureSprite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="texture">An optional texture.</param>
        public Cursor(Texture texture = null)
            : base("cursor", texture, default)
        {
        }

        /// <summary>
        /// Gets a value indicating whether or not the mouse is currently down.
        /// </summary>
        public MouseStatus MouseStatus { get; private set; } = MouseStatus.Up;

        /// <summary>
        /// Gets the current value of the scroll wheel.
        /// </summary>
        public ScrollWheelStatus ScrollWheelStatus { get; private set; } = ScrollWheelStatus.None;

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="x">The x position using ranges 0.0 - 1.0.</param>
        /// <param name="y">The y position using ranges 0.0 - 1.0.</param>
        public void SetPosition(float x, float y)
        {
            NativeFunction.Natives.xFC695459D4D0E219(x, y);
        }

        /// <summary>
        /// Draws the cursor.
        /// </summary>
        /// <param name="g">The Rage.Graphics object to draw against.</param>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture == null)
            {
                var xScale = Game.Resolution.Width / Constants.CanvasWidth;
                var yScale = Game.Resolution.Height / Constants.CanvasHeight;
                g.DrawFilledCircle(new Vector2(this.Position.X * xScale, this.Position.Y * yScale), 10f, Color.Green);
            }
            else
            {
                g.DrawTexture(this.Texture, this.RectF);
            }
        }

        /// <summary>
        /// Updates the cursor's location, click status, and scroll wheel status.
        /// </summary>
        public void Update()
        {
            var x = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorX);
            var y = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorY);
            this.Position = new Point((int)System.Math.Round(x * Constants.CanvasWidth), (int)System.Math.Round(y * Constants.CanvasHeight));
            this.Refresh(new PointF(0f, 0f), 1f);

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
