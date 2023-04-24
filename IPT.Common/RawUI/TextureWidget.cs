using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a texture drawable that can be resized and moved.
    /// </summary>
    public abstract class TextureWidget : TextureDrawable, IWidget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureWidget"/> class.
        /// </summary>
        /// <param name="texture">The texture to be rendered.</param>
        public TextureWidget(Texture texture)
            : base(texture)
        {
        }

        /// <inheritdoc/>
        public PointF DragOffset { get; protected set; } = new PointF(0, 0);

        /// <inheritdoc/>
        public bool IsDragging { get; protected set; } = false;

        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public bool IsHovered { get; set; } = false;

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

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                if (this.IsDragging || this.IsHovered)
                {
                    var highlight = this.Bounds;
                    highlight.Inflate(3f * this.Parent.Scale.Height, 2f * this.Parent.Scale.Height);
                    g.DrawRectangle(highlight, this.IsDragging ? Constants.DraggingColor : Constants.HoverColor);
                }

                g.DrawTexture(this.Texture, this.Bounds);
            }
        }

        /// <inheritdoc/>
        public void EndDrag()
        {
            this.IsDragging = false;
            this.DragOffset = default;
        }

        /// <inheritdoc/>
        public void StartDrag(PointF mousePosition)
        {
            this.IsDragging = true;
            this.DragOffset = new PointF(this.Position.X - mousePosition.X, this.Position.Y - mousePosition.Y);
        }
    }
}
