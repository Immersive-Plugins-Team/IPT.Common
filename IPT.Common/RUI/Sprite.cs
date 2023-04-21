using System.Drawing;
using Rage;

namespace IPT.Common.RUI
{
    /// <summary>
    /// Represents a sprite element that renders a texture to the screen.
    /// </summary>
    public class Sprite : ITextureElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="name">A name the sprite can be referred to as.</param>
        /// <param name="texture">The texture to be rendered.</param>
        public Sprite(string name, Texture texture)
        {
            this.Name = name;
            this.Texture = texture;
        }

        /// <inheritdoc/>
        public virtual bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets a unique name to use for referencing the sprite.
        /// </summary>
        public virtual string Name { get; set; }

        /// <inheritdoc/>
        public virtual Point Position { get; set; }

        /// <inheritdoc/>
        public virtual Texture Texture { get; set; }

        /// <inheritdoc/>
        public virtual IRenderableContainer Parent { get; set; }

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public virtual void Draw(Rage.Graphics g)
        {
            if (this.Texture != null && this.Parent != null)
            {
                var screenPosition = new PointF((this.Parent.Position.X * this.Parent.Scale) + (this.Position.X * this.Parent.Scale), (this.Parent.Position.Y * this.Parent.Scale) + (this.Position.Y * this.Parent.Scale));
                var size = new SizeF(this.Texture.Size.Width * this.Parent.Scale, this.Texture.Size.Height * this.Parent.Scale);
                g.DrawTexture(this.Texture, new RectangleF(screenPosition, size));
            }
        }
    }
}
