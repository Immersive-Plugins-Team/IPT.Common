using System.Drawing;
using IPT.Common.RawUI.Interfaces;
using Rage;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a button with text draw on top of it.
    /// </summary>
    public class TextButton : Button, IText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextButton"/> class.
        /// </summary>
        /// <param name="id">A unique identifer for the button.</param>
        /// <param name="texture">The background texture.</param>
        /// <param name="text">The text drawn on top of the texture.</param>
        public TextButton(string id, Texture texture, string text)
            : base(id, texture)
        {
            this.Text = text;
        }

        /// <inheritdoc/>
        public Color FontColor { get; set; } = Color.White;

        /// <inheritdoc/>
        public string FontFamily { get; set; } = "Lucida Console";

        /// <inheritdoc/>
        public float FontSize { get; set; } = 14f;

        /// <inheritdoc/>
        public float ScaledFontSize { get; protected set; } = 14f;

        /// <inheritdoc/>
        public string Text { get; set; } = string.Empty;

        /// <inheritdoc/>
        public SizeF TextSize { get; protected set; }

        /// <summary>
        /// Gets or sets the real screen position to draw the text.
        /// </summary>
        public PointF TextPosition { get; protected set; } = default;

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            base.Draw(g);
            g.DrawText(this.Text, this.FontFamily, this.ScaledFontSize, this.TextPosition, this.FontColor, this.Bounds);
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                base.UpdateBounds();
                this.UpdateText(this.Parent.Scale.Height);
            }
        }

        /// <summary>
        /// Updates the text metadata.
        /// </summary>
        /// <param name="scale">The parent scale.</param>
        protected virtual void UpdateText(float scale)
        {
            // centers the text on the button
            this.ScaledFontSize = this.FontSize * scale;
            this.TextSize = Rage.Graphics.MeasureText(this.Text, this.FontFamily, this.ScaledFontSize);
            var x = this.Bounds.X + (this.Bounds.Width / 2) - (this.TextSize.Width / 2);
            var y = this.Bounds.Y + (this.Bounds.Height / 2) - (this.TextSize.Height / 2);
            this.TextPosition = new PointF(x, y);
        }
    }
}
