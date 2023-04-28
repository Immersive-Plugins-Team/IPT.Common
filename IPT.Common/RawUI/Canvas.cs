using System;
using System.Drawing;
using IPT.Common.API;
using IPT.Common.Fibers;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.Util;
using Rage;
using Rage.Native;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A canvas representing the screen area where elements can be added and positioned.
    /// </summary>
    public sealed class Canvas : GenericFiber, IParent
    {
        private readonly WidgetManager widgetManager;
        private readonly string texturePath;
        private bool isControlsEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        /// <param name="texturePath">The fully qualified path to the textures.</param>
        public Canvas(string texturePath)
            : base("canvas", 100)
        {
            this.texturePath = texturePath;
            this.UUID = Guid.NewGuid().ToString();
            this.widgetManager = new WidgetManager();
            this.Cursor = new Cursor()
            {
                Parent = this,
            };
        }

        /// <inheritdoc />
        public RectangleF Bounds { get; private set; }

        /// <summary>
        /// Gets or sets the cursor belonging to the canvas.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the canvas is active.
        /// </summary>
        public bool IsActive { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether or not the game is paused.
        /// This is safe to check on the raw frame render event.
        /// </summary>
        public bool IsGamePaused { get; private set; } = false;

        /// <inheritdoc />
        public Point Position { get; } = new Point(0, 0);

        /// <summary>
        /// Gets the screen resolution.
        /// </summary>
        public Size Resolution { get; private set; }

        /// <inheritdoc />
        public SizeF Scale { get; private set; }

        /// <inheritdoc />
        public string UUID { get; private set; }

        /// <summary>
        /// Adds a widget to the canvas.
        /// </summary>
        /// <param name="widget">The widget to add.</param>
        public void Add(IWidget widget)
        {
            widget.Parent = this;
            this.widgetManager.AddWidget(widget);
        }

        /// <summary>
        /// Set the canvas to interactive mode.
        /// </summary>
        /// <param name="doPause">Whether or not to pause the game when doing into interactive mode.</param>
        public void Interact(bool doPause)
        {
            if (this.IsActive)
            {
                return;
            }

            this.IsActive = true;
            this.SetPlayerControls(false);
        }

        /// <inheritdoc />
        public void MoveTo(Point position)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();
            TextureHandler.Load(this.UUID, this.texturePath);
            this.UpdateBounds();
            Rage.Game.FrameRender += this.Game_FrameRender;
            Rage.Game.RawFrameRender += this.Game_RawFrameRender;
        }

        /// <inheritdoc />
        public override void Stop()
        {
            base.Stop();
            this.widgetManager.ClearWidgets();
            Rage.Game.FrameRender -= this.Game_FrameRender;
            Rage.Game.RawFrameRender -= this.Game_RawFrameRender;
        }

        /// <summary>
        /// Executed every time the fiber fires.
        /// </summary>
        protected override void DoSomething()
        {
            this.IsGamePaused = Functions.IsGamePaused();
            if (this.Resolution != Game.Resolution)
            {
                Logging.Info("game resolution has changed, updating");
                this.UpdateBounds();
            }
        }

        private void Game_FrameRender(object sender, Rage.GraphicsEventArgs e)
        {
            if (!this.IsActive || Game.Console.IsOpen || this.IsGamePaused)
            {
                return;
            }
            else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CellphoneCancel))
            {
                this.SetPlayerControls(true);
                this.IsActive = false;
            }
            else
            {
                this.Cursor.UpdateStatus();
                this.widgetManager.HandleMouseEvents(this.Cursor);
            }
        }

        private void Game_RawFrameRender(object sender, Rage.GraphicsEventArgs e)
        {
            if (!this.IsGamePaused)
            {
                this.widgetManager.Draw(e.Graphics);
                if (this.IsActive)
                {
                    this.Cursor.Draw(e.Graphics);
                }
            }
        }

        /// <summary>
        /// Enables or disables the player controls.
        /// </summary>
        /// <param name="isEnabled">Whether or not to enable or disable the controls.</param>
        private void SetPlayerControls(bool isEnabled)
        {
            if (isEnabled != this.isControlsEnabled)
            {
                NativeFunction.Natives.x8D32347D6D4C40A2(Game.LocalPlayer, isEnabled, 0);
                this.isControlsEnabled = isEnabled;
            }
        }

        private void UpdateBounds()
        {
            Logging.Info("game resolution has changed, updating bounds");
            this.Resolution = Rage.Game.Resolution;
            this.Scale = new SizeF(this.Resolution.Width / Constants.CanvasWidth, this.Resolution.Height / Constants.CanvasHeight);
            this.Bounds = new RectangleF(this.Position, this.Resolution);
            this.widgetManager.UpdateWidgetBounds();
        }
    }
}
