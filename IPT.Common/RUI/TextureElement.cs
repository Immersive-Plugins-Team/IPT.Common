using System.Drawing;
using Rage;

namespace IPT.Common.RUI
{
    /// <summary>
    /// Represents a sprite element that renders a texture to the screen.
    /// </summary>
    public abstract class TextureElement : IElement
    {
        private Point position;
        private Texture texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureElement"/> class.
        /// </summary>
        /// <param name="name">A name the sprite can be referred to as.</param>
        /// <param name="texture">The texture to be rendered.</param>
        public TextureElement(string name, Texture texture)
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
        public virtual Point Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                this.Update();
            }
        }

        /// <summary>
        /// Gets or sets the texture drawn by the element.
        /// </summary>
        public virtual Texture Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                this.texture = value;
                this.Update();
            }
        }

        /// <inheritdoc/>
        public virtual IElementContainer Parent { get; set; }

        /// <summary>
        /// Gets or sets the value of the bounding box for drawing the texture.
        /// </summary>
        protected virtual RectangleF BoundingBox { get; set; }

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public virtual void Draw(Rage.Graphics g)
        {
            if (this.Texture != null && this.Parent != null)
            {
                g.DrawTexture(this.Texture, this.BoundingBox);
            }
        }

        /// <summary>
        /// Updates the RectangleF used to draw the texture based on the texture itself, the element's position, and the parent frame's scale.
        /// </summary>
        public void Update()
        {
            var screenPosition = new PointF((this.Parent.Position.X * this.Parent.Scale) + (this.Position.X * this.Parent.Scale), (this.Parent.Position.Y * this.Parent.Scale) + (this.Position.Y * this.Parent.Scale));
            var size = new SizeF(this.Texture.Size.Width * this.Parent.Scale, this.Texture.Size.Height * this.Parent.Scale);
            this.BoundingBox = new RectangleF(screenPosition, size);
        }
    }
}
