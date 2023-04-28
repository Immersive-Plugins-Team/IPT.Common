using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.API;
using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Widgets
{
    /// <summary>
    /// Represents a widget that contains other widgets which allows the user to switch between them.
    /// </summary>
    public class TabbedWidget : BaseWidget
    {
        private readonly Dictionary<string, IWidget> widgets = new Dictionary<string, IWidget>();
        private readonly ToggleButtonPanelWidget buttonPanel;
        private string activeTabTitle = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedWidget"/> class.
        /// </summary>
        /// <param name="activeButtonTextureName">The name of the texture for the active button.</param>
        /// <param name="inactiveButtonTextureName">The name of the texture for the inactive button.</param>
        public TabbedWidget(string activeButtonTextureName, string inactiveButtonTextureName)
        {
            this.buttonPanel = new ToggleButtonPanelWidget(activeButtonTextureName, inactiveButtonTextureName);
            this.buttonPanel.MoveTo(new Point(0, 0));
            this.Add(this.buttonPanel);
            this.buttonPanel.AddObserver(this);
            this.Width = 0;
            this.Height = 0;
        }

        /// <summary>
        /// Properly adds a widget to the tabbed widget.
        /// </summary>
        /// <param name="widget">The widget to add.</param>
        /// <param name="tabTitle">The title to use for the tab.</param>
        public void AddWidget(IWidget widget, string tabTitle)
        {
            this.Add(widget);
            if (this.widgets.Count == 0)
            {
                this.activeTabTitle = tabTitle;
            }

            this.widgets[tabTitle] = widget;
            this.buttonPanel.AddButton(tabTitle, tabTitle);
        }

        /// <inheritdoc/>
        public override bool Contains(Cursor cursor)
        {
            return this.Items.OfType<IWidget>().Any(widget => widget.Contains(cursor));
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.Bounds, Color.LimeGreen);
            this.buttonPanel.Draw(g);
            if (this.widgets.TryGetValue(this.activeTabTitle, out IWidget widget))
            {
                widget.Draw(g);
            }
        }

        /// <inheritdoc />
        public override void OnUpdated(IObservable obj)
        {
            if (obj is ToggleButtonPanelWidget)
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

            // update ourselves
            float x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
            float y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
            var size = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
            this.Bounds = new RectangleF(new PointF(x, y), size);

            // update the panel
            this.buttonPanel.UpdateBounds();

            // now reposition and reupdate the widgets
            foreach (var widget in this.widgets.Values)
            {
                widget.MoveTo(new Point(0, this.buttonPanel.Height));
                widget.UpdateBounds();
            }

            // finally update our bounds again
        }
    }
}
