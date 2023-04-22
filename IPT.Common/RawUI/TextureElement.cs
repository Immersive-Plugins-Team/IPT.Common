using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a drawable item that renders a texture to the screen.
    /// </summary>
    public class TextureElement : IElement, IDraggable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureElement"/> class.
        /// </summary>
        /// <param name="texture">The texture to be rendered.</param>
        public TextureElement(Texture texture)
        {
            this.Texture = texture;
        }

        /// <inheritdoc/>
        public virtual RectangleF Bounds { get; protected set; }

        /// <inheritdoc/>
        public Point DragOffset { get; protected set; }

        /// <inheritdoc/>
        public bool IsDragging { get; protected set; }

        /// <inheritdoc/>
        public virtual bool IsVisible { get; set; } = true;

        /// <inheritdoc/>
        public virtual IContainer Parent { get; set; }

        /// <inheritdoc/>
        public virtual Point Position { get; protected set; }

        /// <summary>
        /// Gets or sets the texture drawn by the element.
        /// </summary>
        public virtual Texture Texture { get; protected set; } = null;

        /// <inheritdoc/>
        public void Drag(PointF mousePosition)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public virtual void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                g.DrawTexture(this.Texture, this.Bounds);
            }
        }

        /// <inheritdoc/>
        public void EndDrag()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void MoveTo(Point position)
        {
            this.Position = position;
            this.Update();
        }

        /// <summary>
        /// Sets the texture.
        /// </summary>
        /// <param name="texture">The texture to use.</param>
        public void SetTexture(Texture texture)
        {
            this.Texture = texture;
            this.Update();
        }

        /// <inheritdoc/>
        public void StartDrag(PointF mousePosition)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void Update()
        {
            // todo = i dont' think this is right either, we need to be drawing relative to the parent's on screen position, not its canvas position
            var screenPosition = new PointF((this.Parent.Position.X * this.Parent.Scale) + (this.Position.X * this.Parent.Scale), (this.Parent.Position.Y * this.Parent.Scale) + (this.Position.Y * this.Parent.Scale));
            var size = new SizeF(this.Texture.Size.Width * this.Parent.Scale, this.Texture.Size.Height * this.Parent.Scale);
            this.Bounds = new RectangleF(screenPosition, size);
        }
    }
}
