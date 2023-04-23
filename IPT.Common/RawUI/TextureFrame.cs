using System.Collections.Generic;
using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// An interactive frame with a texture background that contains IElement objects.
    /// </summary>
    public class TextureFrame : TextureWidget, IContainer<IElement>
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
        public virtual SizeF Scale
        {
            get
            {
                var frameScale = this.FrameScale / 100f;
                return new SizeF(this.Parent.Scale.Width * frameScale, this.Parent.Scale.Height * frameScale);
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
                float y = this.Position.Y * this.Scale.Height;
                if (this.Position.X > (Constants.CanvasWidth / 2f))
                {
                    x = canvas.Resolution.Width - ((Constants.CanvasWidth - this.Position.X - (this.Texture.Size.Width * frameScale)) * canvas.Scale.Width) + (this.Texture.Size.Width * this.Scale.Height);
                }
                else
                {
                    x = this.Position.X * this.Scale.Width;
                }

                var size = new SizeF(this.Texture.Size.Width * this.Scale.Height, this.Texture.Size.Height * this.Scale.Height);
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
