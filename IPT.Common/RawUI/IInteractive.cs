namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents an element that can be interacted with by the user.
    /// </summary>
    public interface IInteractive : IDrawable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the element is enabled and can respond to user interaction.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the element is currently being hovered over by the mouse cursor.
        /// </summary>
        bool IsHovered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the element is currently being clicked on by the mouse cursor.
        /// </summary>
        bool IsPressed { get; set; }
    }
}
