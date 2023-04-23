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
        private bool isInteractive;
        private bool isPaused;
        private bool isControlsEnabled;
        private IInteractive hoveredItem = null;  // mouse is currently hovering over that item
        private IInteractive activeItem = null;   // mouse is currently clicked on that item

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        public Canvas()
            : base("canvas", 100)
        {
            this.Cursor = new Cursor(null);
            this.Items = new List<IDrawable>();
            this.Update();
        }

        /// <inheritdoc />
        public RectangleF Bounds { get; protected set; }

        /// <summary>
        /// Gets or sets the cursor belonging to the canvas.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <summary>
        /// Gets the list of items contained within the container.
        /// </summary>
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

        /// <summary>
        /// Gets the screen resolution.
        /// </summary>
        public Size Resolution { get; private set; }

        /// <inheritdoc />
        public float Scale { get; private set; }

        /// <inheritdoc />
        public void Add(IDrawable item)
        {
            item.Parent = this;
            this.Items.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
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
                this.Cursor.IsVisible = true;
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
        public void Remove(IDrawable element)
        {
            this.Items.Remove(element);
        }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();
            this.Scale = this.Resolution.Height / Constants.CanvasHeight;
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
            this.Resolution = Game.Resolution;
            this.Scale = this.Resolution.Height / Constants.CanvasHeight;
            new RectangleF(this.Position, this.Resolution);
            this.Bounds = new RectangleF(0, 0, this.Resolution.Width, this.Resolution.Height);
            this.Items.ForEach(x => x.Update());
            this.Cursor.Update();
        }

        /// <summary>
        /// Executed every time the thread fires.
        /// </summary>
        protected override void DoSomething()
        {
            this.isPaused = Functions.IsGamePaused();
            if (Game.Resolution != this.Resolution)
            {
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
                this.ReleaseInteractiveElements();
                this.Cursor.IsVisible = false;
            }
            else
            {
                this.Cursor.UpdateStatus();
                this.UpdateInteractiveItems();
            }
        }

        private void ReleaseInteractiveElements()
        {
            this.hoveredItem = null;
            this.activeItem = null;
            foreach (var item in this.Items.OfType<IInteractive>())
            {
                item.IsHovered = false;
                item.EndDrag();
                item.EndResize();
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

        private void UpdateInteractiveItems()
        {
            bool hoveredItemFound = false;

            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if (this.Items[i] is IInteractive interactiveItem)
                {
                    if (!hoveredItemFound && interactiveItem.Bounds.Contains(this.Cursor.Position))
                    {
                        interactiveItem.IsHovered = true;
                        hoveredItemFound = true;
                        this.hoveredItem = interactiveItem;
                    }
                    else
                    {
                        interactiveItem.IsHovered = false;
                    }
                }
            }

            if (!hoveredItemFound)
            {
                this.hoveredItem = null;
            }
        }
    }
}
