using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A drawable texture.
    /// </summary>
    public class Sprite : TextureElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <param name="texture">The underlying texture.</param>
        public Sprite(string name, Texture texture)
            : base(name, texture)
        {
        }
    }
}
