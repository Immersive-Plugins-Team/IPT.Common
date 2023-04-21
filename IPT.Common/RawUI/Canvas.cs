using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.API;
using IPT.Common.Fibers;
using Rage;
using Rage.Native;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A canvas representing the screen area where elements can be added and positioned.
    /// </summary>
    public class Canvas : GenericFiber, IContainer
    {
        private readonly Point position = new Point(0, 0);

        private Size resolution;
        private bool isInteractive;
        private bool isPaused;
        private bool isControlsEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        public Canvas()
            : base("canvas", 100)
        {
            this.Cursor = new Cursor(null);
        }

        /// <inheritdoc />
        public RectangleF Bounds { get { return new RectangleF(this.Position, new SizeF(this.resolution.Width, this.resolution.Height)); } }

        /// <summary>
        /// Gets or sets the cursor belonging to the canvas.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <inheritdoc />
        public List<IDrawable> Items { get; private set; }

        /// <inheritdoc />
        public bool IsVisible { get; set; }

        /// <inheritdoc />
        public IContainer Parent
        {
            get { return null; }
            set { }
        }

        /// <inheritdoc />
        public Point Position
        {
            get { return this.position; }
            set { }
        }

        /// <inheritdoc />
        public float Scale { get; private set; }

        /// <inheritdoc />
        public void AddItem(IDrawable element)
        {
            this.Items.Add(element);
        }

        /// <inheritdoc />
        public void ClearItems()
        {
            this.Items.Clear();
        }

        /// <inheritdoc />
        public void Draw(Rage.Graphics g)
        {
            this.Items.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(g));
            if (this.isInteractive)
            {
                this.Cursor.Draw(g);
            }
        }

        /// <summary>
        /// Set the canvas to interactive mode.
        /// </summary>
        /// <param name="doPause">Whether or not to pause the game when doing into interactive mode.</param>
        public void Interact(bool doPause)
        {
            if (!Functions.IsGamePaused() && !this.isInteractive)
            {
                this.isInteractive = true;
                if (doPause)
                {
                    Game.IsPaused = true;
                }
                else
                {
                    this.SetPlayerControls(false);
                }
            }
        }

        /// <inheritdoc />
        public void MoveTo(Point position)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void RemoveItem(IDrawable element)
        {
            this.Items.Remove(element);
        }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();
            this.Scale = this.resolution.Height / Constants.CanvasHeight;
            Game.FrameRender += this.Game_FrameRender;
            Game.RawFrameRender += this.Game_RawFrameRender;
        }

        /// <inheritdoc />
        public override void Stop()
        {
            base.Stop();
            Game.FrameRender -= this.Game_FrameRender;
            Game.RawFrameRender -= this.Game_RawFrameRender;
        }

        /// <inheritdoc />
        public void Update()
        {
            this.Scale = this.resolution.Height / Constants.CanvasHeight;
            this.Items.ForEach(x => x.Update());
            this.Cursor.Update();
        }

        /// <summary>
        /// Executed every time the thread fires.
        /// </summary>
        protected override void DoSomething()
        {
            this.isPaused = Functions.IsGamePaused();
            if (Game.Resolution != this.resolution)
            {
                this.resolution = Game.Resolution;
                this.Update();
            }
        }

        private void Game_FrameRender(object sender, GraphicsEventArgs e)
        {
            if (this.isInteractive)
            {
                this.ProcessControls();
            }
        }

        private void Game_RawFrameRender(object sender, GraphicsEventArgs e)
        {
            if (!this.isPaused || this.isInteractive)
            {
                this.Draw(e.Graphics);
            }
        }

        private void ProcessControls()
        {
            if (Game.Console.IsOpen)
            {
                return;
            }
            else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CellphoneCancel))
            {
                this.isInteractive = false;
                Game.IsPaused = false;
                this.SetPlayerControls(true);
            }
            else
            {
                this.Cursor.UpdateStatus();
            }
        }

        private void SetPlayerControls(bool isEnabled)
        {
            if (isEnabled != this.isControlsEnabled)
            {
                NativeFunction.Natives.x8D32347D6D4C40A2(Game.LocalPlayer, isEnabled, 0);
                this.isControlsEnabled = isEnabled;
            }
        }
    }
}
