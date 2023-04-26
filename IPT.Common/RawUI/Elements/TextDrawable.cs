using System.Drawing;
using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a drawable text based element.
    /// </summary>
    public abstract class TextDrawable : IDrawable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextDrawable"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public TextDrawable(string text)
        {
            this.Text = text;
        }

        /// <inheritdoc/>
        public RectangleF Bounds { get; protected set; } = default;

        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        public Color FontColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string FontFamily { get; set; } = "Lucida Console";

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public float FontSize { get; set; } = 14f;

        /// <inheritdoc/>
        public bool IsVisible { get; set; } = true;

        /// <inheritdoc/>
        public IParent Parent { get; set; }

        /// <inheritdoc/>
        public Point Position { get; protected set; } = default;

        /// <summary>
        /// Gets or sets the text to be drawn.
        /// </summary>
        public string Text { get; set; }

        /// <inheritdoc/>
        public virtual void Draw(Rage.Graphics g)
        {
            g.DrawText(this.Text, this.FontFamily, this.FontSize, this.Bounds.Location, this.FontColor, this.Bounds);
        }

        /// <inheritdoc/>
        public void MoveTo(Point position)
        {
            this.Position = position;
            this.UpdateBounds();
        }

        /// <inheritdoc/>
        public virtual void UpdateBounds()
        {
            if (this.Parent != null)
            {
                var screenPosition = new PointF(this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height), this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height));
                var realSize = Rage.Graphics.MeasureText(this.Text, this.FontFamily, this.FontSize);
                var scaledSize = new SizeF(realSize.Width * this.Parent.Scale.Height, realSize.Height * this.Parent.Scale.Height);
                this.Bounds = new RectangleF(screenPosition, scaledSize);
            }
        }
    }
}
