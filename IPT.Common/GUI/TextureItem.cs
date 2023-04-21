using System.Drawing;
using Rage;

namespace IPT.Common.GUI
{
    /// <summary>
    /// An abstract class for a texture-related item.
    /// </summary>
    public abstract class TextureItem
    {
        /// <summary>
        /// Gets or sets a value indicating the name of the texture item.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the texture item is visible.
        /// </summary>
        public virtual bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the texture item's position.
        /// </summary>
        public virtual Point Position { get; protected set; }

        /// <summary>
        /// Gets or sets the texture position's RectF which is used to draw the texture.
        /// </summary>
        public virtual RectangleF RectF { get; protected set; }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        public virtual Texture Texture { get; protected set; }

        /// <summary>
        /// Draws the texture item.
        /// </summary>
        /// <param name="g">The Rage.Graphics object to use for the drawing.</param>
        public abstract void Draw(Rage.Graphics g);

        /// <summary>
        /// Moves the texture item to the given position which should be coordinates on the base canvas.
        /// </summary>
        /// <param name="position">The target position.</param>
        public virtual void MoveTo(Point position)
        {
            this.Position = position;
        }

        /// <summary>
        /// Toggles the visibility of the frame.
        /// </summary>
        public virtual void Toggle()
        {
            this.IsVisible = !this.IsVisible;
        }
    }
}
