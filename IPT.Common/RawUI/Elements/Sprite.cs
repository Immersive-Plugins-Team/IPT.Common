using Rage;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// An implementation of a drawable texture.
    /// </summary>
    public class Sprite : TextureDrawable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="texture">The underlying texture.</param>
        public Sprite(Texture texture)
            : base(texture)
        {
        }
    }
}
