using System.Drawing;
using Rage;

namespace IPT.Common.GUI
{
    /// <summary>
    /// A sprite for textures.
    /// </summary>
    public class TextureSprite : TextureItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureSprite"/> class.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <param name="texture">The texture of the sprite.</param>
        /// <param name="position">The relative (to its parent frame) position.</param>
        public TextureSprite(string name, Texture texture, Point position)
        {
            this.Name = name;
            this.Texture = texture;
            this.Position = position;
            this.RectF = default;
            this.Refresh(new PointF(0f, 0f), 1f);
            this.IsVisible = false;
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="g">The Rage.Graphics object to draw on.</param>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null && this.IsVisible)
            {
                g.DrawTexture(this.Texture, this.RectF);
            }
        }

        /// <summary>
        /// Refreshes the underlying sprite rect.
        /// </summary>
        /// <param name="framePosition">The absolute position of the parent frame.</param>
        /// <param name="scale">The scale of the parent frame.</param>
        public void Refresh(PointF framePosition, float scale)
        {
            scale *= Game.Resolution.Height / Constants.CanvasHeight;
            var size = new SizeF(this.Texture.Size.Width * scale, this.Texture.Size.Height * scale);
            this.RectF = new RectangleF(new PointF(framePosition.X + (this.Position.X * scale), framePosition.Y + (this.Position.Y * scale)), size);
        }
    }
}
