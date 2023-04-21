using Rage;

namespace IPT.Common.RUI
{
    /// <summary>
    /// Represents an RUI element that renders a texture to the screen.
    /// </summary>
    public interface ITextureElement : IRenderable
    {
        /// <summary>
        /// Gets or sets the texture to be rendered.
        /// </summary>
        Texture Texture { get; set; }
    }
}
