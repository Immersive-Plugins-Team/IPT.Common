using System.Drawing;

namespace IPT.Common.RUI
{
    /// <summary>
    /// Represents an object that can be rendered to the screen.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Gets or sets the position of the object relative to its parent container.
        /// </summary>
        Point Position { get; set; }

        /// <summary>
        /// Gets or sets the parent container of this element.
        /// </summary>
        IElementContainer Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this element is visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Draws the object to the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        void Draw(Rage.Graphics g);

        /// <summary>
        /// Updates the underlying bounding box based on the position and scale.
        /// </summary>
        void Update();
    }
}
