using System.Drawing;
using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a drawable text based element.
    /// </summary>
    public abstract class TextDrawable : IDrawable, IText
    {
        /// <inheritdoc/>
        public RectangleF Bounds { get; protected set; } = default;

        /// <inheritdoc/>
        public Color FontColor { get; set; } = Color.Black;

        /// <inheritdoc/>
        public string FontFamily { get; set; } = "Lucida Console";

        /// <inheritdoc/>
        public float FontSize { get; set; } = 14f;

        /// <inheritdoc/>
        public bool IsVisible { get; set; } = true;

        /// <inheritdoc/>
        public IParent Parent { get; set; }

        /// <inheritdoc/>
        public Point Position { get; protected set; } = default;

        /// <inheritdoc/>
        public float ScaledFontSize { get; protected set; } = 14f;

        /// <inheritdoc/>
        public string Text { get; set; } = string.Empty;

        /// <inheritdoc/>
        public SizeF TextSize { get; protected set; } = default;

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
                this.TextSize = Rage.Graphics.MeasureText(this.Text, this.FontFamily, this.ScaledFontSize);
                this.Bounds = new RectangleF(screenPosition, this.TextSize);
            }
        }
    }
}
