using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents an interactive texture element.
    /// </summary>
    public abstract class InteractiveTextureElement : TextureElement, IInteractive
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveTextureElement"/> class.
        /// </summary>
        /// <param name="texture">The texture to be rendered.</param>
        public InteractiveTextureElement(Texture texture)
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
        public bool IsResizing { get; protected set; } = false;

        /// <inheritdoc/>
        public PointF ResizeOffset { get; protected set; } = new PointF(0, 0);

        /// <inheritdoc/>
        public abstract void Click();

        /// <inheritdoc/>
        public void Drag(PointF mousePosition)
        {
            this.MoveTo(new Point((int)System.Math.Round(mousePosition.X - this.DragOffset.X), (int)System.Math.Round(mousePosition.Y - this.DragOffset.Y)));
        }

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                if (this.IsDragging)
                {
                    var highlight = this.Bounds;
                    highlight.Inflate(2f * this.Parent.Scale, 2f * this.Parent.Scale);
                    g.DrawRectangle(highlight, Constants.HighlightColor);
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
        public void EndResize()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void Resize(PointF mousePosition)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void StartDrag(PointF mousePosition)
        {
            this.IsDragging = true;
            this.DragOffset = new PointF(this.Position.X - mousePosition.X, this.Position.Y - mousePosition.Y);
        }

        /// <inheritdoc/>
        public void StartResize(PointF mousePosition)
        {
            throw new System.NotImplementedException();
        }
    }
}
