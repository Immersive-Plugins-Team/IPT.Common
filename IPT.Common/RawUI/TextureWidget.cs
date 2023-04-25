using System.Collections.Generic;
using System.Drawing;
using IPT.Common.API;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// An interactive frame with a texture background that contains IDrawable objects.
    /// </summary>
    public abstract class TextureWidget : TextureDrawable, IWidget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureWidget"/> class.
        /// </summary>
        /// <param name="texture">The widget's texture.</param>
        public TextureWidget(Texture texture)
            : base(texture)
        {
        }

        /// <inheritdoc/>
        public PointF DragOffset { get; protected set; } = new PointF(0, 0);

        /// <inheritdoc/>
        public bool IsDragging { get; protected set; } = false;

        /// <inheritdoc/>
        public virtual bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public virtual bool IsHovered { get; set; } = false;

        /// <summary>
        /// Gets or sets the list of the items contained within the container.
        /// </summary>
        public List<IDrawable> Items { get; protected set; } = new List<IDrawable>();

        /// <inheritdoc />
        public SizeF Scale
        {
            get
            {
                return new SizeF(this.Parent.Scale.Width * this.WidgetScale, this.Parent.Scale.Height * this.WidgetScale);
            }
        }

        /// <inheritdoc/>
        public float WidgetScale { get; protected set; } = 1.0f;

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
        public abstract void Click();

        /// <inheritdoc/>
        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        /// <inheritdoc/>
        public void Drag(PointF mousePosition)
        {
            this.MoveTo(new Point((int)System.Math.Round(mousePosition.X + this.DragOffset.X), (int)System.Math.Round(mousePosition.Y + this.DragOffset.Y)));
        }

        /// <inheritdoc />
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                base.Draw(g);
            }

            foreach (var item in this.Items)
            {
                if (item.IsVisible)
                {
                    item.Draw(g);
                }
            }
        }

        /// <inheritdoc />
        public void Remove(IDrawable item)
        {
            this.Items.Remove(item);
        }

        /// <inheritdoc />
        public void SetWidgetScale(float scale)
        {
            if (this.Parent is Canvas)
            {
                this.WidgetScale = API.Math.Clamp(scale, Constants.MinScale, Constants.MaxScale);
                this.UpdateBounds();
            }
        }

        /// <inheritdoc/>
        public void StartDrag(PointF mousePosition)
        {
            this.IsDragging = true;
            this.DragOffset = new PointF(this.Position.X - mousePosition.X, this.Position.Y - mousePosition.Y);
        }

        /// <inheritdoc/>
        public void StopDrag()
        {
            this.IsDragging = false;
            this.DragOffset = default;
        }

        /// <inheritdoc />
        public override void UpdateBounds()
        {
            if (this.Texture != null)
            {
                if (this.Parent is Canvas canvas)
                {
                    float x = this.Position.X * canvas.Scale.Width;
                    float y = this.Position.Y * canvas.Scale.Height;
                    var size = new SizeF(this.Texture.Size.Width * this.Scale.Height, this.Texture.Size.Height * this.Scale.Height);
                    this.Bounds = new RectangleF(new PointF(x, y), size);
                }
                else
                {
                    base.UpdateBounds();
                }
            }
            else
            {
                Logging.Warning("cannout update bounds on widget, texture is null!");
            }

            foreach (IDrawable item in this.Items)
            {
                item.UpdateBounds();
            }
        }
    }
}
