using System.Collections.Generic;
using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A frame containing multiple sprites.
    /// </summary>
    /// <typeparam name="T">The type of the sprites that the frame contains.</typeparam>
    public class Frame<T> : TextureElement, IContainer
        where T : IElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Frame{T}"/> class.
        /// </summary>
        /// <param name="texture">The underlying texture.</param>
        public Frame(Texture texture)
            : base(texture)
        {
            this.FrameScale = 100;
        }

        /// <summary>
        /// Gets or sets the list of the items contained within the container.
        /// </summary>
        public List<IDrawable> Items { get; protected set; }

        /// <summary>
        /// Gets or sets the frame specific scale applied after the parent container's (e.g. canvas) scale.
        /// </summary>
        public virtual int FrameScale { get; protected set; }

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
        public void Add(IDrawable item)
        {
            if (item is T || item.GetType().IsSubclassOf(typeof(T)))
            {
                this.Items.Add(item);
            }
            else
            {
                throw new System.ArgumentException("SpriteFrame only allows elements of type Sprite or its subclasses.", "element");
            }
        }

        /// <inheritdoc />
        public void Remove(IDrawable item)
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
            this.FrameScale = API.Math.Clamp(scale, Constants.MinScale, Constants.MaxScale);
            this.Update();
        }

        /// <inheritdoc />
        public override void Update()
        {
            var screenPosition = new PointF((this.Parent.Position.X * this.Scale) + (this.Position.X * this.Scale), (this.Parent.Position.Y * this.Scale) + (this.Position.Y * this.Scale));
            var size = new SizeF(this.Texture.Size.Width * this.Scale, this.Texture.Size.Height * this.Scale);
            this.Bounds = new RectangleF(screenPosition, size);

            foreach (IDrawable item in this.Items)
            {
                item.Update();
            }
        }
    }
}
