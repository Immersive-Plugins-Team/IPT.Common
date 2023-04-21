using System.Collections.Generic;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A container for sprites.
    /// </summary>
    /// <typeparam name="T">The type of drawables that the frame contains.</typeparam>
    public class TextureFrame<T> : TextureItem, IContainer
        where T : Sprite
    {
        private List<IDrawable> items;
        private int frameScale;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFrame{T}"/> class.
        /// </summary>
        /// <param name="texture">The underlying texture.</param>
        public TextureFrame(Texture texture)
            : base(texture)
        {
            this.items = new List<IDrawable>();
            this.frameScale = 100;
        }

        /// <inheritdoc />
        public List<IDrawable> Items
        {
            get { return this.items; }
            private set { this.items = value; }
        }

        /// <summary>
        /// Gets or sets the frame specific scale applied after the parent container's (e.g. canvas) scale.
        /// </summary>
        public virtual int FrameScale
        {
            get
            {
                return this.frameScale;
            }

            set
            {
                this.frameScale = API.Math.Clamp(value, Constants.MinScale, Constants.MaxScale);
                this.Update();
            }
        }

        /// <inheritdoc />
        public virtual float Scale
        {
            get
            {
                return this.Parent.Scale * (this.frameScale / 100f);
            }
        }

        /// <inheritdoc />
        public override void Draw(Rage.Graphics g)
        {
            base.Draw(g);
            foreach (IDrawable item in this.items)
            {
                if (item.IsVisible)
                {
                    item.Draw(g);
                }
            }
        }

        /// <inheritdoc />
        public void AddItem(IDrawable item)
        {
            if (item is T || item.GetType().IsSubclassOf(typeof(T)))
            {
                this.items.Add(item);
            }
            else
            {
                throw new System.ArgumentException("Frame only allows IElement or its subclasses.", "item");
            }
        }

        /// <inheritdoc />
        public void RemoveItem(IDrawable item)
        {
            this.items.Remove(item);
        }

        /// <inheritdoc />
        public void ClearItems()
        {
            this.items.Clear();
        }

        /// <inheritdoc />
        public override void Update()
        {
            base.Update();
            foreach (IDrawable item in this.items)
            {
                item.Update();
            }
        }
    }
}
