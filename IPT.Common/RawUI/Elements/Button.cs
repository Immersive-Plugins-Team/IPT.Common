using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a clickable sprite.
    /// </summary>
    public abstract class Button : Sprite, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="texture">The button's texture.</param>
        public Button(Rage.Texture texture)
            : base(texture)
        {
        }

        /// <inheritdoc/>
        public virtual bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public abstract void Click();

        /// <inheritdoc/>
        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }
    }
}
