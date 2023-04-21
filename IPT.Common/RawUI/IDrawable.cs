using System.Drawing;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents an item that can be drawn to the screen.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this item is visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the position of the item relative to its parent container.
        /// </summary>
        Point Position { get; set; }

        /// <summary>
        /// Gets or sets the parent container of this item.
        /// </summary>
        IContainer Parent { get; set; }

        /// <summary>
        /// Draws the item to the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        void Draw(Rage.Graphics g);

        /// <summary>
        /// Updates the underlying bounding box based on the position and scale.
        /// </summary>
        void Update();
    }
}
