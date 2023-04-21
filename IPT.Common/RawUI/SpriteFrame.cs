using System.Collections.Generic;
using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A frame containing multiple sprites.
    /// </summary>
    /// <typeparam name="T">The type of the sprites that the frame contains.</typeparam>
    public class SpriteFrame<T> : TextureElement, IElementContainer
        where T : Sprite
    {
        private List<IElement> elements;
        private int frameScale;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFrame{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the sprite frame.</param>
        /// <param name="texture">The underlying texture.</param>
        public SpriteFrame(string name, Texture texture)
            : base(name, texture)
        {
            this.elements = new List<IElement>();
            this.frameScale = 100;
        }

        /// <inheritdoc />
        public List<IElement> Elements
        {
            get { return this.elements; }
            private set { this.elements = value; }
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
            foreach (IElement element in this.elements)
            {
                if (element.IsVisible)
                {
                    element.Draw(g);
                }
            }
        }

        /// <inheritdoc />
        public void AddElement(IElement element)
        {
            if (element is T || element.GetType().IsSubclassOf(typeof(T)))
            {
                this.elements.Add(element);
            }
            else
            {
                throw new System.ArgumentException("SpriteFrame only allows elements of type Sprite or its subclasses.", "element");
            }
        }

        /// <inheritdoc />
        public void RemoveElement(IElement element)
        {
            this.elements.Remove(element);
        }

        /// <inheritdoc />
        public void ClearElements()
        {
            this.elements.Clear();
        }

        /// <inheritdoc />
        public override void Update()
        {
            var screenPosition = new PointF((this.Parent.Position.X * this.Scale) + (this.Position.X * this.Scale), (this.Parent.Position.Y * this.Scale) + (this.Position.Y * this.Scale));
            var size = new SizeF(this.Texture.Size.Width * this.Scale, this.Texture.Size.Height * this.Scale);
            this.BoundingBox = new RectangleF(screenPosition, size);

            foreach (IElement element in this.elements)
            {
                element.Update();
            }
        }
    }
}
