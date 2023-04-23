using System.Diagnostics;
using System.Drawing;
using IPT.Common.API;
using Rage;
using Rage.Native;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A special sprite that represents the mouse cursor on the screen.
    /// </summary>
    public class Cursor : Sprite
    {
        private readonly Stopwatch clickTimer = new Stopwatch();
        private bool isVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="texture">The cursor's texture.</param>
        public Cursor(Texture texture)
            : base(texture)
        {
            this.MouseStatus = MouseStatus.Up;
            this.ScrollWheelStatus = ScrollWheelStatus.None;
        }

        /// <summary>
        /// Gets the duration of a mouse down event.
        /// </summary>
        public long ClickDuration
        {
            get
            {
                return this.clickTimer.ElapsedMilliseconds;
            }
        }

        /// <inheritdoc/>
        public override bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                this.isVisible = value;
                this.clickTimer.Reset();
            }
        }

        /// <summary>
        /// Gets or sets the mouse down or up status.
        /// </summary>
        public MouseStatus MouseStatus { get; protected set; }

        /// <summary>
        /// Gets or sets the mouse scroll wheel down, up, or none status.
        /// </summary>
        public ScrollWheelStatus ScrollWheelStatus { get; protected set; }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                base.Draw(g);
            }
            else
            {
                // draw a crosshair instead
            }
        }

        /// <summary>
        /// Updates the position, mouse status, and scroll wheel status.  The position is based on the canvas.
        /// </summary>
        public void UpdateStatus()
        {
            this.UpdatePosition();
            this.UpdateMouseStatus();
            this.UpdateScrollWheelStatus();
        }

        /// <summary>
        /// Updates the status of the mouse button.
        /// </summary>
        protected void UpdateMouseStatus()
        {
            if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.Attack))
            {
                if (this.MouseStatus != MouseStatus.Down)
                {
                    this.clickTimer.Restart();
                    this.MouseStatus = MouseStatus.Down;
                }
            }
            else
            {
                if (this.MouseStatus != MouseStatus.Up)
                {
                    this.clickTimer.Stop();
                    this.MouseStatus = MouseStatus.Up;
                }
            }
        }

        /// <summary>
        /// Updates the cursor's position based on the mouse position.
        /// </summary>
        protected void UpdatePosition()
        {
            var x = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorX);
            var y = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorY);
            this.Position = new Point((int)System.Math.Round(x * Constants.CanvasWidth), (int)System.Math.Round(y * Constants.CanvasHeight));
        }

        /// <summary>
        /// Update's the status of the scroll wheel.
        /// </summary>
        protected void UpdateScrollWheelStatus()
        {
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
