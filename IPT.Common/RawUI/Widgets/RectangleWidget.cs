using System.Drawing;

namespace IPT.Common.RawUI.Widgets
{
    /// <summary>
    /// Represents a plain colored rectangular widget.
    /// </summary>
    public class RectangleWidget : BaseWidget
    {
        /// <summary>
        /// Gets or sets the background color of the rectangle.
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.FromArgb(128, 64, 64, 64);

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            base.Draw(g);
        }
    }
}
