using System.Drawing;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents an item that can be drawn to the screen.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Gets the real screen area in which to place the drawable.
        /// </summary>
        RectangleF Bounds { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the parent container of this item.
        /// </summary>
        IContainer Parent { get; set; }

        /// <summary>
        /// Gets the position of the item relative to its parent container.
        /// </summary>
        Point Position { get; }

        /// <summary>
        /// Draws the item to the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        void Draw(Rage.Graphics g);

        /// <summary>
        ///  Moves the drawable to the given coordinates on the canvas.
        /// </summary>
        /// <param name="position">The base canvas coordinates.</param>
        void MoveTo(Point position);

        /// <summary>
        /// Updates the bounds based on the internal rules of the drawable.
        /// </summary>
        void Update();
    }
}
