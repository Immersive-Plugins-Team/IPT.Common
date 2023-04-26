using System.Drawing;
using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a drawable text based element.
    /// </summary>
    public abstract class TextDrawable : IDrawable
    {
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
        /// Gets or sets the font size scaled to the parent widget.
        /// </summary>
        public float ScaledFontSize { get; protected set; } = 14f;

        /// <summary>
        /// Gets or sets the text to be drawn.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text size.
        /// </summary>
        public float TextHeight { get; protected set; } = 14f;

        /// <inheritdoc/>
        public abstract void Draw(Rage.Graphics g);

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
                this.ScaledFontSize = this.FontSize * this.Parent.Scale.Height;
                var textSize = Rage.Graphics.MeasureText(this.Text, this.FontFamily, this.ScaledFontSize);
                this.Bounds = new RectangleF(screenPosition, textSize);
                this.TextHeight = textSize.Height;
            }
        }
    }
}
