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
        private bool isInteractive = false;
        private bool isPaused = false;
        private bool isControlsEnabled = true;
        private IWidget hoveredWidget = null;        // mouse is currently hovering over that widget
        private IWidget activeWidget = null;         // mouse is currently down on that widget
        private IControl hoveredControl = null;      // mouse is currently hovering over that control
        private MouseStatus mouseStatus = MouseStatus.Up;

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        public Canvas()
            : base("canvas", 100)
        {
            this.Cursor = new Cursor(null);
            this.Add(this.Cursor);
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
        public SizeF Scale { get; private set; }

        /// <inheritdoc />
        public void Add(IDrawable item)
        {
            item.Parent = this;
            this.Items.Add(item);
            Logging.Debug($"adding item to canvas, total items: {this.Items.Count}");
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <inheritdoc />
        public void Draw(Rage.Graphics g)
        {
            foreach (var item in this.Items)
            {
                if (item.IsVisible)
                {
                    item.Draw(g);
                }
            }

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
                this.mouseStatus = MouseStatus.Up;
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
        /// Executed every time the thread fires.
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
                this.ReleaseWidgets();
                this.Cursor.IsVisible = false;
            }
            else
            {
                this.Cursor.UpdateStatus();
                this.UpdateWidgets();
                this.mouseStatus = this.Cursor.MouseStatus;
            }
        }

        private void ReleaseWidgets()
        {
            this.Cursor.SetCursorType(CursorType.Default);
            this.activeWidget = null;
            this.hoveredControl = null;
            this.hoveredWidget = null;
            foreach (var item in this.Items.OfType<IWidget>())
            {
                item.IsHovered = false;
                item.StopDrag();
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

        private void UpdateActiveWidget()
        {
            if (this.Cursor.MouseStatus == MouseStatus.Down)
            {
                if (this.mouseStatus == MouseStatus.Down)
                {
                    if (this.activeWidget != null)
                    {
                        if (this.activeWidget.IsDragging)
                        {
                            this.activeWidget.Drag(this.Cursor.Position);
                            if (this.Cursor.ScrollWheelStatus == ScrollWheelStatus.Up)
                            {
                                this.activeWidget.SetWidgetScale(this.activeWidget.WidgetScale + 0.1f);
                            }
                            else if (this.Cursor.ScrollWheelStatus == ScrollWheelStatus.Down)
                            {
                                this.activeWidget.SetWidgetScale(this.activeWidget.WidgetScale - 0.1f);
                            }
                        }
                        else
                        {
                            if (this.activeWidget.Contains(this.Cursor))
                            {
                                if (this.Cursor.ClickDuration > Constants.LongClick)
                                {
                                    Logging.Debug("starting drag...");
                                    Logging.Debug($"active widget position: {this.activeWidget.Position}, bounds: {this.activeWidget.Bounds}");
                                    Logging.Debug($"cursor position       : {this.Cursor.Position}, bounds: {this.Cursor.Bounds}");
                                    this.activeWidget.StartDrag(this.Cursor.Position);
                                    Logging.Debug($"widget drag offset    : {this.activeWidget.DragOffset}");
                                }
                            }
                            else
                            {
                                this.activeWidget = null;
                            }
                        }
                    }
                }
                else
                {
                    if (this.hoveredWidget != null)
                    {
                        this.activeWidget = this.hoveredWidget;
                    }
                }
            }
            else
            {
                if (this.mouseStatus != MouseStatus.Up)
                {
                    if (this.activeWidget != null)
                    {
                        if (this.activeWidget.IsDragging)
                        {
                            Logging.Debug("ending drag...");
                            Logging.Debug($"active widget position: {this.activeWidget.Position}, bounds: {this.activeWidget.Bounds}");
                            Logging.Debug($"cursor position       : {this.Cursor.Position}, bounds: {this.Cursor.Bounds}");
                            this.activeWidget.StopDrag();
                        }
                        else if (this.hoveredControl != null)
                        {
                            Logging.Debug("click!");
                            this.hoveredControl.Click();
                        }

                        this.activeWidget = null;
                    }
                }
            }
        }

        private void UpdateHoveredWidget()
        {
            bool hoveredItemFound = false;
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                var item = this.Items[i];
                Logging.Debug($"checking item {i}: {item.GetType().FullName}");
                if (this.Items[i] is IWidget widget)
                {
                    Logging.Debug($"checking item {i} to see if it contains cursor");
                    if (!hoveredItemFound && widget.Contains(this.Cursor))
                    {
                        Logging.Debug($"found hovered widget at {this.Cursor.Bounds}");
                        widget.IsHovered = true;
                        hoveredItemFound = true;
                        this.hoveredWidget = widget;
                        Logging.Info("searching hovered widget to see if it contains any controls");
                        var controls = ControlFinder.FindControls(this.hoveredWidget).ToList();
                        Logging.Info($"we found {controls.Count} controls");
                        if (controls.Count > 0)
                        {
                            Logging.Info("checking to see if any contain the cursor");
                            var hoveredControl = controls.Where(x => x.Contains(this.Cursor)).ToList().LastOrDefault();
                            if (hoveredControl != null)
                            {
                                Logging.Info("we found one, setting it to the hoveredControl");
                                this.hoveredControl = hoveredControl;
                            }
                            else
                            {
                                Logging.Info("none of the controls are being hovered over");
                            }
                        }
                    }
                    else
                    {
                        widget.IsHovered = false;
                    }
                }
            }

            if (!hoveredItemFound)
            {
                this.hoveredControl = null;
                this.hoveredWidget = null;
            }

            this.Cursor.SetCursorType(this.hoveredControl == null ? CursorType.Default : CursorType.Pointing);
        }

        private void UpdateWidgets()
        {
            this.UpdateHoveredWidget();
            this.UpdateActiveWidget();
        }
    }
}
