using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.RawUI.Elements;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.Util;

namespace IPT.Common.RawUI.Widgets
{
    /// <summary>
    /// Represents a widget that holds toggled buttons.
    /// </summary>
    public class ToggleButtonPanelWidget : RectangleWidget, IObservable
    {
        private readonly List<IObserver> observers = new List<IObserver>();
        private readonly string activeButtonTextureName;
        private readonly string inactiveButtonTextureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleButtonPanelWidget"/> class.
        /// </summary>
        /// <param name="activeButtonTextureName">The name of the texture for the active button.</param>
        /// <param name="inactiveButtonTextureName">The name of the texture for the inactive button.</param>
        public ToggleButtonPanelWidget(string activeButtonTextureName, string inactiveButtonTextureName)
            : base(0, 0)
        {
            this.activeButtonTextureName = activeButtonTextureName;
            this.inactiveButtonTextureName = inactiveButtonTextureName;
            this.BackgroundColor = Color.Yellow;
        }

        /// <inheritdoc/>
        public string Id { get; protected set; } = string.Empty;

        /// <inheritdoc/>
        public override void Add(IDrawable item)
        {
            if (item is ToggledButton)
            {
                base.Add(item);
            }
        }

        /// <summary>
        /// Adds a button to the widget.
        /// </summary>
        /// <param name="id">The unique name of the button.</param>
        /// <param name="text">The text to write on the button.</param>
        public void AddButton(string id, string text)
        {
            var button = new ToggledButton(id, this.activeButtonTextureName, this.inactiveButtonTextureName, text, true);
            button.AddObserver(this);
            this.Add(button);
            if (this.Items.Count == 1)
            {
                button.IsActive = true;
            }
        }

        /// <inheritdoc/>
        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <inheritdoc />
        public override void OnUpdated(IObservable obj)
        {
            this.Id = obj.Id;
            foreach (var button in this.Items.OfType<ToggledButton>())
            {
                if (button.Id != obj.Id)
                {
                    button.IsActive = false;
                }
            }

            this.observers.ForEach(observer => observer.OnUpdated(this));
        }

        /// <inheritdoc/>
        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                var texture = TextureHandler.Get(this.Parent.UUID, this.activeButtonTextureName);
                if (texture != null)
                {
                    this.Width = texture.Size.Width * this.Items.Count;
                    this.Height = texture.Size.Height;
                }
            }

            base.UpdateBounds();
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i] is ToggledButton button)
                {
                    button.MoveTo(new System.Drawing.Point(button.Width * i, 0));
                    button.UpdateBounds();
                }
            }
        }
    }
}
