using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common;
using IPT.Common.API;
using Rage;
using Rage.Native;

namespace IPT.Common.GUI
{
    /// <summary>
    /// A class for managing one or more texture frames.
    /// </summary>
    public class BaseFrame : IPT.Common.Fibers.GenericFiber
    {
        private readonly List<TextureFrame> frames = new List<TextureFrame>();

        private Size resolution;
        private bool isEditing;
        private bool isPaused;
        private bool isControlsEnabled;
        private TextureFrame mousedFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFrame"/> class.
        /// </summary>
        public BaseFrame()
            : base("baseframe", 100)
        {
            this.mousedFrame = null;
            this.resolution = default;
            this.isEditing = false;
            this.isPaused = false;
            this.isControlsEnabled = true;
            this.Cursor = new Cursor();
        }

        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        public Cursor Cursor { get; protected set; }

        /// <summary>
        /// Adds a texture frame to the base frame.
        /// </summary>
        /// <param name="frame">The frame to add.</param>
        public void AddFrame(TextureFrame frame)
        {
            this.frames.Add(frame);
        }

        /// <summary>
        /// Moves any frames matching the given name.
        /// </summary>
        /// <param name="name">The name of the frame.</param>
        /// <param name="position">The new position of the frame.</param>
        public void MoveFrame(string name, Point position)
        {
            this.frames.Where(frame => frame.Name == name).ToList().ForEach(frame => frame.MoveTo(position));
        }

        /// <summary>
        /// Rescales any frames matching the given name.
        /// </summary>
        /// <param name="name">The name of the frame.</param>
        /// <param name="scale">The new scale of the frame.</param>
        public void RescaleFrame(string name, int scale)
        {
            this.frames.Where(frame => frame.Name == name).ToList().ForEach((frame) => frame.Scale = scale);
        }

        /// <summary>
        /// Changes into an edit mode where the textures can be respositioned and resized.
        /// </summary>
        /// <param name="pause">Whether or not to pause the game when edit mode is activated.</param>
        public void EditMode(bool pause)
        {
            if (!Functions.IsGamePaused() && !this.isEditing)
            {
                this.isEditing = true;
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
            if (this.frames.Count == 0)
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
                this.frames.ForEach(frame => frame.Refresh());
            }
        }

        /// <summary>
        /// Draws a border around the screen.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        protected void DrawBorder(Rage.Graphics g)
        {
            var scale = Game.Resolution.Height / Constants.CanvasHeight;
            var width = scale * 10f;
            g.DrawRectangle(new RectangleF(0, 0, Game.Resolution.Width, width), Color.Green);
            g.DrawRectangle(new RectangleF(0, Game.Resolution.Height - width + 2, Game.Resolution.Width, width), Color.Green);
            g.DrawRectangle(new RectangleF(0, 0, width, Game.Resolution.Height), Color.Green);
            g.DrawRectangle(new RectangleF(Game.Resolution.Width - width + 1, 0, width, Game.Resolution.Height), Color.Green);
            g.DrawText("**EDIT MODE - RIGHT-CLICK ANYWHERE TO EXIT**", "Consolas", 50f * scale, new PointF(20, 20), Color.White);
        }

        /// <summary>
        /// Called when the base frame is in editing mode.
        /// </summary>
        protected void ProcessEditingControls()
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
            this.UpdateEditingStatus();
            if (this.isEditing)
            {
                this.ProcessEditingControls();
            }
        }

        /// <summary>
        /// Called by the RawFrameRender event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected void Game_RawFrameRender(object sender, GraphicsEventArgs e)
        {
            if (this.isEditing)
            {
                this.DrawBorder(e.Graphics);
                this.frames.ForEach(x => x.Draw(e.Graphics));
                this.Cursor.Draw(e.Graphics);
            }
            else if (!this.isPaused)
            {
                this.frames.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(e.Graphics));
            }
        }

        /// <summary>
        /// When the user scrolls the mouse, this applies a rescaling factor to the frames.
        /// </summary>
        protected void RescaleFrames()
        {
            if (this.Cursor.RescaleFactor != 0)
            {
                this.frames.LastOrDefault(frame => frame.Contains(this.Cursor))?.Rescale(this.Cursor.RescaleFactor);
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
        /// Called during the FrameRender event.
        /// </summary>
        protected void UpdateEditingStatus()
        {
            if (this.isEditing)
            {
                if (Game.Console.IsOpen)
                {
                    this.isEditing = false;
                }
                else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CellphoneCancel))
                {
                    this.isEditing = false;
                }

                if (!this.isEditing)
                {
                    this.frames.ForEach(x => x.Drop());
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
                if (this.Cursor.IsMouseDown)
                {
                    this.mousedFrame.MoveTo(this.Cursor);
                }
                else
                {
                    this.mousedFrame.Drop();
                    this.mousedFrame = null;
                }
            }
            else if (this.Cursor.IsMouseDown)
            {
                var frame = this.frames.LastOrDefault(x => x.Contains(this.Cursor));
                if (frame != null)
                {
                    this.mousedFrame = frame;
                    this.mousedFrame.Lift(this.Cursor);
                }
            }
        }
    }
}
