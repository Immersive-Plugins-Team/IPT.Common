using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.API;
using Rage;
using Rage.Native;

namespace IPT.Common.GUI
{
    /// <summary>
    /// A class for managing one or more texture frames.
    /// </summary>
    /// <typeparam name="T">The Frame type.</typeparam>
    public class BaseFrame<T> : Fibers.GenericFiber
        where T : TextureFrame
    {
        private Size resolution;
        private bool isInteractive;
        private bool isPaused;
        private bool isControlsEnabled;
        private TextureFrame mousedFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFrame{T}"/> class.
        /// </summary>
        public BaseFrame()
            : base("baseframe", 100)
        {
            this.mousedFrame = null;
            this.resolution = default;
            this.isInteractive = false;
            this.isPaused = false;
            this.isControlsEnabled = true;
            this.Cursor = new Cursor();
            this.Frames = new List<T>();
        }

        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        public Cursor Cursor { get; protected set; }

        /// <summary>
        /// Gets or sets a list of texture frames on the base frame.  They are drawn in order such that the last frame in the list will be on top.
        /// </summary>
        public virtual List<T> Frames { get; protected set; }

        /// <summary>
        /// Adds a texture frame to the base frame.
        /// </summary>
        /// <param name="frame">The frame to add.</param>
        public void AddFrame(T frame)
        {
            this.Frames.Add(frame);
        }

        /// <summary>
        /// Moves any frames matching the given name.
        /// </summary>
        /// <param name="name">The name of the frame.</param>
        /// <param name="position">The new position of the frame.</param>
        public void MoveFrame(string name, Point position)
        {
            this.Frames.Where(frame => frame.Name == name).ToList().ForEach(frame => frame.MoveTo(position));
        }

        /// <summary>
        /// Rescales any frames matching the given name.
        /// </summary>
        /// <param name="name">The name of the frame.</param>
        /// <param name="scale">The new scale of the frame.</param>
        public void RescaleFrame(string name, int scale)
        {
            this.Frames.Where(frame => frame.Name == name).ToList().ForEach((frame) => frame.Scale = scale);
        }

        /// <summary>
        /// Changes into an interactive mode where the textures can be interacted with.
        /// </summary>
        /// <param name="pause">Whether or not to pause the game when interactive mode is activated.</param>
        public virtual void Interact(bool pause)
        {
            if (!Functions.IsGamePaused() && !this.isInteractive)
            {
                this.isInteractive = true;
                this.Cursor.SetPosition(new PointF(0.5f, 0.5f));
                if (pause)
                {
                    Game.IsPaused = true;
                }
                else
                {
                    this.SetPlayerControls(false);
                }
            }
        }

        /// <summary>
        /// Starts the fiber.
        /// </summary>
        public override void Start()
        {
            Logging.Info("Starting base frame");
            if (this.Frames.Count == 0)
            {
                Logging.Warning("there are no texture frames to manage!");
            }
            else
            {
                Game.FrameRender += this.Game_FrameRender;
                Game.RawFrameRender += this.Game_RawFrameRender;
                base.Start();
            }
        }

        /// <summary>
        /// Stops the fiber.
        /// </summary>
        public override void Stop()
        {
            Logging.Info("stopping base frame");
            Game.FrameRender -= this.Game_FrameRender;
            Game.RawFrameRender -= this.Game_RawFrameRender;
            base.Stop();
        }

        /// <summary>
        /// Fired during every fiber tick.
        /// </summary>
        protected override void DoSomething()
        {
            this.isPaused = Functions.IsGamePaused();
            if (this.resolution != Game.Resolution)
            {
                this.resolution = Game.Resolution;
                this.Frames.ForEach(frame => frame.Refresh());
            }
        }

        /// <summary>
        /// Draws a border around the screen.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="color">The border color.</param>
        /// <param name="thickness">The border thickness.</param>
        /// <param name="showMessage">Whether or not to show the edit mode message.</param>
        protected void DrawBorder(Rage.Graphics g, Color color, float thickness = 10f, bool showMessage = true)
        {
            var scale = Game.Resolution.Height / Constants.CanvasHeight;
            thickness *= scale;
            g.DrawRectangle(new RectangleF(0, 0, Game.Resolution.Width, thickness), color);
            g.DrawRectangle(new RectangleF(0, Game.Resolution.Height - thickness + 2, Game.Resolution.Width, thickness), color);
            g.DrawRectangle(new RectangleF(0, 0, thickness, Game.Resolution.Height), color);
            g.DrawRectangle(new RectangleF(Game.Resolution.Width - thickness + 1, 0, thickness, Game.Resolution.Height), color);
            if (showMessage)
            {
                g.DrawText("**EDIT MODE - RIGHT-CLICK ANYWHERE TO EXIT**", "Consolas", 50f * scale, new PointF(20, 20), Color.White);
            }
        }

        /// <summary>
        /// Called on the RawFrameRender event while in interactive mode.
        /// </summary>
        /// <param name="g">The Rage.Graphics object to draw on.</param>
        protected virtual void DrawInteractiveGraphics(Rage.Graphics g)
        {
            this.DrawBorder(g, Color.Green);
            this.Frames.ForEach(x => x.Draw(g));
            this.Cursor.Draw(g);
        }

        /// <summary>
        /// Called when the base frame is in interactive mode.
        /// </summary>
        protected virtual void ProcessInteractiveControls()
        {
            this.Cursor.Update();
            this.UpdateMousedFrame();
            this.RescaleFrames();
        }

        /// <summary>
        /// Called by the FrameRender event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected void Game_FrameRender(object sender, GraphicsEventArgs e)
        {
            this.UpdateInteractiveStatus();
            if (this.isInteractive)
            {
                this.ProcessInteractiveControls();
            }
        }

        /// <summary>
        /// Called by the RawFrameRender event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected void Game_RawFrameRender(object sender, GraphicsEventArgs e)
        {
            if (this.isInteractive)
            {
                this.DrawInteractiveGraphics(e.Graphics);
            }
            else if (!this.isPaused)
            {
                this.Frames.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(e.Graphics));
            }
        }

        /// <summary>
        /// When the user scrolls the mouse, this applies a rescaling factor to the frames.
        /// </summary>
        protected void RescaleFrames()
        {
            // if (this.Cursor.RescaleFactor != 0)
            if (this.Cursor.ScrollWheelStatus != ScrollWheelStatus.None)
            {
                this.Frames.LastOrDefault(frame => frame.Contains(this.Cursor))?.Rescale(this.Cursor.ScrollWheelStatus == ScrollWheelStatus.Up ? 1 : -1);
            }
        }

        /// <summary>
        /// Enables or disables the player controls.
        /// </summary>
        /// <param name="isEnabled">A value indicating whether to enable or disable the controls.</param>
        protected void SetPlayerControls(bool isEnabled)
        {
            if (isEnabled != this.isControlsEnabled)
            {
                NativeFunction.Natives.x8D32347D6D4C40A2(Game.LocalPlayer, isEnabled, 0);
                this.isControlsEnabled = isEnabled;
            }
        }

        /// <summary>
        /// Call this during the FrameRender event.
        /// </summary>
        protected void UpdateInteractiveStatus()
        {
            if (this.isInteractive)
            {
                if (Game.Console.IsOpen)
                {
                    this.isInteractive = false;
                }
                else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CellphoneCancel))
                {
                    this.isInteractive = false;
                }

                if (!this.isInteractive)
                {
                    this.Frames.ForEach(x => x.Drop());
                    Game.IsPaused = false;
                    this.SetPlayerControls(true);
                }
            }
        }

        /// <summary>
        /// Updates the frame currently under the mouse control.
        /// </summary>
        protected void UpdateMousedFrame()
        {
            if (this.mousedFrame != null)
            {
                if (this.Cursor.MouseStatus == MouseStatus.Down)
                {
                    this.mousedFrame.MoveTo(this.Cursor);
                }
                else
                {
                    this.mousedFrame.Drop();
                    this.mousedFrame = null;
                }
            }
            else if (this.Cursor.MouseStatus == MouseStatus.Down)
            {
                var frame = this.Frames.LastOrDefault(x => x.Contains(this.Cursor));
                if (frame != null)
                {
                    this.mousedFrame = frame;
                    this.mousedFrame.Lift(this.Cursor);
                }
            }
        }
    }
}
