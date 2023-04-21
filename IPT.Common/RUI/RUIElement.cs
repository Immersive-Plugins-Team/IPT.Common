using System.Drawing;

namespace IPT.Common.RUI
{
    /// <summary>
    /// Represents a basic UI element that can be rendered to a <see cref="RUI.Canvas"/>.
    /// </summary>
    public abstract class RUIElement : IRenderable
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the element is visible.
        /// </summary>
        public virtual bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the position of the element relative to its parent container.
        /// </summary>
        public virtual PointF Position { get; set; }

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public abstract void Draw(Rage.Graphics g);
    }
}
