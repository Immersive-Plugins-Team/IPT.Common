using System.Drawing;
using Rage;

namespace IPT.Common.GUI
{
    /// <summary>
    /// A sprite for textures.
    /// </summary>
    public class TextureSprite
    {
        private readonly Texture texture;
        private readonly Point position;
        private RectangleF spriteRect;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureSprite"/> class.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <param name="texture">The texture of the sprite.</param>
        /// <param name="position">The relative (to its parent frame) position.</param>
        public TextureSprite(string name, Texture texture, Point position)
        {
            this.Name = name;
            this.texture = texture;
            this.position = position;
            this.Refresh(new PointF(0f, 0f), 1f);
        }

        /// <summary>
        /// Gets a value indicating the name of the sprite.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="g">The Rage.Graphics object to draw on.</param>
        public void Draw(Rage.Graphics g)
        {
            if (this.texture != null)
            {
                g.DrawTexture(this.texture, this.spriteRect);
            }
        }

        /// <summary>
        /// Refreshes the underlying sprite rect.
        /// </summary>
        /// <param name="framePosition">The absolute position of the parent frame.</param>
        /// <param name="scale">The scale of the parent frame.</param>
        internal void Refresh(PointF framePosition, float scale)
        {
            scale *= Game.Resolution.Height / Constants.CanvasHeight;
            var size = new SizeF(this.texture.Size.Width * scale, this.texture.Size.Height * scale);
            this.spriteRect = new RectangleF(new PointF(framePosition.X + (this.position.X * scale), framePosition.Y + (this.position.Y * scale)), size);
        }
    }
}
