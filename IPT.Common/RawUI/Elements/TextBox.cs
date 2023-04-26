using System.Drawing;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a box with text in it.
    /// </summary>
    public class TextBox : TextDrawable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="text">The text in the box.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width of the text box.</param>
        /// <param name="height">The height of the text box.</param>
        public TextBox(string text, int x, int y, int width, int height)
            : base(text)
        {
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        /// <summary>
        /// Gets or sets the color of the text box.
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        public Color BorderColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        public float BorderWidth { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the height of the text box.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the text box.
        /// </summary>
        public int Width { get; set; }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            var borderRect = this.Bounds;
            borderRect.Inflate(this.BorderWidth, this.BorderWidth);
            g.DrawRectangle(borderRect, this.BorderColor);
            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            var fontSize = this.Parent != null ? (this.Parent.Scale.Height * this.FontSize) : this.FontSize;
            g.DrawText(this.Text, this.FontFamily, fontSize, this.Bounds.Location, this.FontColor, this.Bounds);
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                var screenPosition = new PointF(this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height), this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height));
                // var realSize = Rage.Graphics.MeasureText(this.Text, this.FontFamily, this.FontSize);
                var scaledSize = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
                this.Bounds = new RectangleF(screenPosition, scaledSize);
            }
        }
    }
}
