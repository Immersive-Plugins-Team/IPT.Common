using System.Drawing;

namespace IPT.Common.RUI
{
    /// <summary>
    /// Represents an object that can be rendered to the screen.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Gets or sets the position of the object relative to its parent container.
        /// </summary>
        Point Position { get; set; }

        /// <summary>
        /// Gets or sets the parent container of this element.
        /// </summary>
        IRenderableContainer Parent { get; set; }

        /// <summary>
        /// Gets a value indicating whether this element is visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Draws the object to the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        void Draw(Rage.Graphics g);
    }
}
