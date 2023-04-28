﻿using System.Collections.Generic;
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
            this.tabWidget = this.BuildTabWidget();
            this.Add(this.tabWidget);
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

        public List<IWidget> Widgets
        {
            get
            {
                return this.widgets.Values.ToList();
            }
        }

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
            this.tabWidget.Add(new RectangleButton(tabTitle, 150, 50, tabTitle) { BackgroundColor = Color.Black, FontColor = Color.White });

            if (widget.Height > this.Height)
            {
                this.Height = widget.Height + 50;
            }

            if (widget.Width > this.Width)
            {
                this.Width = widget.Width;
                this.tabWidget.Resize(new Size(this.Width, 50));
            }

            this.UpdateBounds();
            this.tabWidget.UpdateBounds();
            widget.MoveTo(new System.Drawing.Point(0, 50));
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

        private TabWidget BuildTabWidget()
        {
            var tabWidget = new TabWidget(150, 50) { BackgroundColor = Color.Cyan };
            tabWidget.Parent = this;
            tabWidget.MoveTo(new Point(0, 0));
            tabWidget.AddObserver(this);
            return tabWidget;
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
                drawable.MoveTo(new Point(this.Items.Count * 150, 0));
                base.Add(drawable);
            }

            public void AddObserver(IObserver observer)
            {
                this.observers.Add(observer);
            }

            public override void OnUpdated(IObservable obj)
            {
                Logging.Info($"tab widget received update from {obj.Id}");
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
