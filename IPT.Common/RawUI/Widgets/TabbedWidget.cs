using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.API;
using IPT.Common.RawUI.Elements;
using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Widgets
{
    /// <summary>
    /// Represents a widget that contains other widgets which allows the user to switch between them.
    /// </summary>
    public class TabbedWidget : BaseWidget, IText
    {
        private readonly Dictionary<string, IWidget> widgets = new Dictionary<string, IWidget>();
        private readonly TabWidget tabWidget;
        private string activeTabTitle = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedWidget"/> class.
        /// </summary>
        public TabbedWidget()
        {
            this.tabWidget = new TabWidget(1, 1);
            this.tabWidget.MoveTo(new Point(0, 0));
            this.tabWidget.AddObserver(this);
            this.Width = 0;
            this.Height = 0;
        }

        /// <inheritdoc/>
        public Color FontColor { get; set; } = Color.White;

        /// <inheritdoc/>
        public string FontFamily { get; set; } = "Lucida Console";

        /// <inheritdoc/>
        public float FontSize { get; set; } = 16f;

        /// <inheritdoc/>
        public float ScaledFontSize { get; protected set; } = 16f;

        /// <inheritdoc/>
        public string Text { get; set; } = string.Empty;

        /// <inheritdoc/>
        public SizeF TextSize { get; protected set; } = default;

        /// <summary>
        /// Properly adds a widget to the tabbed widget.
        /// </summary>
        /// <param name="widget">The widget to add.</param>
        /// <param name="tabTitle">The title to use for the tab.</param>
        public void AddWidget(IWidget widget, string tabTitle)
        {
            if (widget is TabbedWidget)
            {
                Logging.Warning("you cannot add tabbed widgets to other tabbed widgets");
            }

            widget.Parent = this;
            if (this.widgets.Count == 0)
            {
                this.activeTabTitle = tabTitle;
            }

            this.widgets[tabTitle] = widget;
            this.tabWidget.Add(new RectangleButton(tabTitle, 50, 20, tabTitle));
            if (widget.Height > this.Height)
            {
                this.Height = widget.Height + 20;
            }

            if (widget.Width > this.Width)
            {
                this.Width = widget.Width;
            }

            this.UpdateBounds();
            widget.MoveTo(new System.Drawing.Point(0, 20));
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            this.tabWidget.Draw(g);
            if (this.widgets.TryGetValue(this.activeTabTitle, out IWidget widget))
            {
                widget.Draw(g);
            }
        }

        /// <inheritdoc />
        public override void OnUpdated(IObservable obj)
        {
            if (obj is RectangleButton)
            {
                this.activeTabTitle = obj.Id;
            }
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.widgets.Count == 0 || this.Parent == null)
            {
                return;
            }

            float x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
            float y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
            var size = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
            this.Bounds = new RectangleF(new PointF(x, y), size);

            this.tabWidget.UpdateBounds();
            this.widgets.Values.ToList().ForEach(widget => widget.UpdateBounds());
        }

        /// <summary>
        /// Hides the base widget's Add method.
        /// </summary>
        /// <param name="item">The IDrawable item to not add.</param>
        protected new void Add(IDrawable item)
        {
            Logging.Warning("you can only add widgets to the tabbed widget using the AddWidget method");
        }

        private class TabWidget : RectangleWidget, IObservable
        {
            private readonly List<IObserver> observers = new List<IObserver>();

            public TabWidget(int width, int height)
                : base(width, height)
            {
            }

            public string Id { get; } = "tabwidget";

            public override void Add(IDrawable drawable)
            {
                drawable.MoveTo(new Point(0, this.Items.Count * 20));
                base.Add(drawable);
            }

            public void AddObserver(IObserver observer)
            {
                this.observers.Add(observer);
            }

            public override void OnUpdated(IObservable obj)
            {
                foreach (IObserver observer in this.observers)
                {
                    observer.OnUpdated(obj);
                }
            }

            public void RemoveObserver(IObserver observer)
            {
                this.observers.Remove(observer);
            }
        }
    }
}
