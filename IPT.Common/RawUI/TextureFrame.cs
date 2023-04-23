﻿using System.Collections.Generic;
using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A frame containing multiple sprites.
    /// </summary>
    public class TextureFrame : InteractiveTextureElement, IContainer<IElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFrame"/> class.
        /// </summary>
        /// <param name="texture">The underlying texture.</param>
        public TextureFrame(Texture texture)
            : base(texture)
        {
        }

        /// <summary>
        /// Gets or sets the list of the items contained within the container.
        /// </summary>
        public List<IElement> Items { get; protected set; } = new List<IElement>();

        /// <summary>
        /// Gets or sets the frame scale applied after the canvas scale.  Should always be 100 if parent is not the canvas.
        /// </summary>
        public virtual int FrameScale { get; protected set; } = 100;

        /// <inheritdoc />
        public virtual float Scale
        {
            get
            {
                return this.Parent.Scale * (this.FrameScale / 100f);
            }
        }

        /// <inheritdoc />
        public override void Draw(Rage.Graphics g)
        {
            base.Draw(g);
            foreach (var item in this.Items)
            {
                if (item.IsVisible)
                {
                    item.Draw(g);
                }
            }
        }

        /// <inheritdoc />
        public void Add(IElement item)
        {
            item.Parent = this;
            this.Items.Add(item);
        }

        /// <inheritdoc />
        public override void Click()
        {
        }

        /// <inheritdoc />
        public void Remove(IElement item)
        {
            this.Items.Remove(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <summary>
        /// Sets the frame scale.
        /// </summary>
        /// <param name="scale">A scale as an integer where 100 = 1.0.</param>
        public virtual void SetFrameScale(int scale)
        {
            if (this.Parent is Canvas)
            {
                this.FrameScale = API.Math.Clamp(scale, Constants.MinScale, Constants.MaxScale);
                this.UpdateBounds();
            }
        }

        /// <inheritdoc />
        public override void UpdateBounds()
        {
            if (this.Parent is Canvas canvas)
            {
                var frameScale = this.FrameScale / 100f;
                float x;
                float y = this.Position.Y * this.Scale;
                if (this.Position.X > (Constants.CanvasWidth / 2f))
                {
                    x = canvas.Resolution.Width - ((Constants.CanvasWidth - this.Position.X - (this.Texture.Size.Width * frameScale)) * (canvas.Resolution.Width / Constants.CanvasWidth)) + (this.Texture.Size.Width * this.Scale);
                }
                else
                {
                    x = this.Position.X * this.Scale;
                }

                var size = new SizeF(this.Texture.Size.Width * this.Scale, this.Texture.Size.Height * this.Scale);
                this.Bounds = new RectangleF(new PointF(x, y), size);
            }
            else
            {
                base.UpdateBounds();
            }

            foreach (IDrawable item in this.Items)
            {
                item.UpdateBounds();
            }
        }
    }
}
