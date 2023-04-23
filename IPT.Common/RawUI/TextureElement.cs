using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a drawable item that renders a texture to the screen.
    /// </summary>
    public abstract class TextureElement : IElement
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
        public virtual RectangleF Bounds { get; protected set; } = default;

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
        public virtual void Update()
        {
            var screenPosition = new PointF(this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale), this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale));
            var size = new SizeF(this.Texture.Size.Width * this.Parent.Scale, this.Texture.Size.Height * this.Parent.Scale);
            this.Bounds = new RectangleF(screenPosition, size);
        }
    }
}
