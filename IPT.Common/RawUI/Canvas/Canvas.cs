﻿using System.Collections.Generic;
using System.Drawing;
using IPT.Common.API;
using IPT.Common.Fibers;
using Rage;

namespace IPT.Common.RawUI.Canvas
{
    /// <summary>
    /// A canvas representing the screen area where elements can be added and positioned.
    /// </summary>
    public class Canvas : GenericFiber, IContainer
    {
        private bool isPaused = false;
        private CanvasState canvasState;
        private MouseState mouseState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        public Canvas()
            : base("canvas", 100)
        {
            this.Cursor = new Cursor(null);
            this.Add(this.Cursor);
            this.canvasState = new InactiveState(this);
            this.mouseState = new MouseUpState(this);
        }

        /// <summary>
        /// Gets or sets the widget currently being clicked or dragged.
        /// </summary>
        public IWidget ActiveWidget { get; set; }

        /// <inheritdoc />
        public RectangleF Bounds { get; protected set; }

        /// <summary>
        /// Gets or sets the cursor belonging to the canvas.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <summary>
        /// Gets or sets the control currently being hovered over.
        /// </summary>
        public IControl HoveredControl { get; set; } = null;

        /// <summary>
        /// Gets or sets the widget currently being hovered over.
        /// </summary>
        public IWidget HoveredWidget { get; set; } = null;

        /// <summary>
        /// Gets the list of items contained within the container.
        /// </summary>
        public List<IDrawable> Items { get; private set; } = new List<IDrawable>();

        /// <inheritdoc />
        public bool IsVisible { get; set; }

        /// <inheritdoc />
        public IParent Parent
        {
            get { return null; }
            set { }
        }

        /// <inheritdoc />
        public Point Position { get; } = new Point(0, 0);

        /// <summary>
        /// Gets the screen resolution.
        /// </summary>
        public Size Resolution { get; private set; }

        /// <inheritdoc />
        public SizeF Scale { get; private set; }

        /// <inheritdoc />
        public void Add(IDrawable item)
        {
            item.Parent = this;
            this.Items.Insert(0, item);
            Logging.Debug($"adding item to canvas, total items: {this.Items.Count}");
        }

        /// <summary>
        /// Moves the item to the top of the z order.
        /// </summary>
        /// <param name="item">The item to bring forward.</param>
        public void BringToFront(IDrawable item)
        {
            // todo
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <inheritdoc />
        public void Draw(Rage.Graphics g)
        {
            this.canvasState.Draw(g);
        }

        /// <summary>
        /// Set the canvas to interactive mode.
        /// </summary>
        /// <param name="doPause">Whether or not to pause the game when doing into interactive mode.</param>
        public void Interact(bool doPause)
        {
            if (!Functions.IsGamePaused() && this.canvasState is InactiveState)
            {
                this.SetCanvasState(new ActiveState(this, doPause));
                this.SetMouseState(new MouseUpState(this));
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

        /// <summary>
        /// Sets the canvas state.
        /// </summary>
        /// <param name="state">The state of the canvas (active or inactive).</param>
        public void SetCanvasState(CanvasState state)
        {
            this.canvasState = state;
        }

        /// <summary>
        /// Sets the canvas state.
        /// </summary>
        /// <param name="state">The state of the canvas (active or inactive).</param>
        public void SetMouseState(MouseState state)
        {
            this.mouseState = state;
        }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();
            this.UpdateBounds();
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
        public void UpdateBounds()
        {
            this.Resolution = Game.Resolution;
            this.Scale = new SizeF(this.Resolution.Width / Constants.CanvasWidth, this.Resolution.Height / Constants.CanvasHeight);
            new RectangleF(this.Position, this.Resolution);
            this.Bounds = new RectangleF(0, 0, this.Resolution.Width, this.Resolution.Height);
            this.Items.ForEach(x => x.UpdateBounds());
        }

        /// <summary>
        /// Executed every time the fiber fires.
        /// </summary>
        protected override void DoSomething()
        {
            this.isPaused = Functions.IsGamePaused();
            if (Game.Resolution != this.Resolution)
            {
                this.UpdateBounds();
            }
        }

        private void Game_FrameRender(object sender, GraphicsEventArgs e)
        {
            this.canvasState.ProcessControls();
            if (this.canvasState is ActiveState)
            {
                this.mouseState.UpdateWidgets();
            }
        }

        private void Game_RawFrameRender(object sender, GraphicsEventArgs e)
        {
            if (!this.isPaused || this.canvasState is ActiveState)
            {
                this.canvasState.Draw(e.Graphics);
            }
        }
    }
}
