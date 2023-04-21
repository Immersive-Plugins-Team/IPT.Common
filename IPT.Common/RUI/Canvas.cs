using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.API;
using IPT.Common.Fibers;
using Rage;
using Rage.Native;

namespace IPT.Common.RUI
{
    /// <summary>
    /// A canvas representing the screen area where RUIElements can be added and positioned.
    /// </summary>
    public class Canvas : GenericFiber, IRenderableContainer
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

        /// <summary>
        /// Gets or sets the cursor belonging to the canvas.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <inheritdoc />
        public List<IRenderable> Elements { get; private set; }

        /// <inheritdoc />
        public bool IsVisible { get; private set; }

        /// <inheritdoc />
        public IRenderableContainer Parent
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
        public float Scale { get; set; }

        /// <inheritdoc />
        public void AddElement(IRenderable element)
        {
            this.Elements.Add(element);
        }

        /// <inheritdoc />
        public void ClearElements()
        {
            this.Elements.Clear();
        }

        /// <inheritdoc />
        public void Draw(Rage.Graphics g)
        {
            this.Elements.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(g));
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
        public void RemoveElement(IRenderable element)
        {
            this.Elements.Remove(element);
        }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();
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

        /// <summary>
        /// Executed every time the thread fires.
        /// </summary>
        protected override void DoSomething()
        {
            if (Game.Resolution != this.resolution)
            {
                this.resolution = Game.Resolution;
                this.Scale = this.resolution.Height / Constants.CanvasHeight;
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
            this.Draw(e.Graphics);
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
                this.Cursor.Update();
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
