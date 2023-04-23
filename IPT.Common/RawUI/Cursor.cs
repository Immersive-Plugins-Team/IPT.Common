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
        private CursorTextureSet? textureSet = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        /// <param name="textureSet">The cursor's texture set.</param>
        public Cursor(CursorTextureSet? textureSet = null)
            : base(null)
        {
            this.textureSet = textureSet;
            this.MouseStatus = MouseStatus.Up;
            this.ScrollWheelStatus = ScrollWheelStatus.None;
            if (this.textureSet != null)
            {
                this.Texture = this.textureSet.Value.Default;
            }
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
                this.SetCursorType(CursorType.Default);
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

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The type of cursor (drag, pointer, resize).</param>
        public void SetCursorType(CursorType cursorType)
        {
            if (this.textureSet != null)
            {
                Texture texture = null;
                switch (cursorType)
                {
                    case CursorType.Default:
                        texture = this.textureSet.Value.Default;
                        break;
                    case CursorType.Pointing:
                        texture = this.textureSet.Value.Pointing;
                        break;
                    case CursorType.Resizing:
                        texture = this.textureSet.Value.Resizing;
                        break;
                }

                this.Texture = texture;
            }
        }

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
        /// Sets the <see cref="CursorTextureSet"/> for the cursor.
        /// </summary>
        /// <param name="textureSet">The cursor's texture set.</param>
        public void SetTextureSet(CursorTextureSet textureSet)
        {
            this.textureSet = textureSet;
            this.SetCursorType(CursorType.Default);
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                var screenPosition = new PointF(this.Position.X * this.Parent.Scale.Width, this.Position.Y * this.Parent.Scale.Height);
                var size = this.Texture == null ? new SizeF(0, 0) : new SizeF(this.Texture.Size.Width * this.Parent.Scale.Height, this.Texture.Size.Height * this.Parent.Scale.Height);
                this.Bounds = new RectangleF(screenPosition, size);
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
            this.UpdateBounds();
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

        /// <summary>
        /// A struct that represents a set of textures for the cursor.
        /// </summary>
        public struct CursorTextureSet
        {
            /// <summary>
            /// Gets or sets the texture for the default cursor.
            /// </summary>
            public Texture Default { get; set; }

            /// <summary>
            /// Gets or sets the texture used when the cursor is pointing.
            /// </summary>
            public Texture Pointing { get; set; }

            /// <summary>
            /// Gets or sets the texture used when the cursor is resizing a widget.
            /// </summary>
            public Texture Resizing { get; set; }
        }
    }
}
