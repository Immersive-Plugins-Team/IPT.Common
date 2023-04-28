﻿using System.Drawing;
using System.Runtime.CompilerServices;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.Util;
using Rage;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a drawable element that renders a texture to the screen.
    /// </summary>
    public abstract class TextureElement : BaseElement, IGeometry
    {
        private readonly bool isDynamic;
        private Texture texture = null;
        private string textureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureElement"/> class.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        /// <param name="width">The width of the element relative to the canvas..</param>
        /// <param name="height">The height of the element relative to the canvas..</param>
        public TextureElement(string textureName, int width, int height)
        {
            this.textureName = textureName;
            this.Width = width;
            this.Height = height;
            this.isDynamic = width == 0 && height == 0;
        }

        /// <inheritdoc/>
        public int Height { get; protected set; }

        /// <summary>
        /// Gets the texture drawn by the element.
        /// </summary>
        public Texture Texture
        {
            get
            {
                if (this.texture == null)
                {
                    if (this.Parent != null && this.textureName != string.Empty)
                    {
                        this.texture = TextureHandler.Get(this.Parent.UUID, this.textureName);
                        this.UpdateBounds();
                    }
                }

                return this.texture;
            }
        }

        /// <inheritdoc/>
        public int Width { get; protected set; }

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                g.DrawTexture(this.Texture, this.Bounds);
            }
        }

        /// <summary>
        /// Sets the texture name.
        /// </summary>
        /// <param name="textureName">The name of the texture to use.</param>
        public void SetTextureName(string textureName)
        {
            this.textureName = textureName;
            this.texture = null;
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Texture == null || this.Parent == null)
            {
                return;
            }

            if (this.isDynamic)
            {
                this.Height = this.Texture.Size.Height;
                this.Width = this.Texture.Size.Width;
            }

            var x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
            var y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
            var size = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
            this.Bounds = new RectangleF(new PointF(x, y), size);
        }
    }
}
