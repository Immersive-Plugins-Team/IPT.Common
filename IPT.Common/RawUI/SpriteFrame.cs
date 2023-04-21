using System.Collections.Generic;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFrame{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the sprite frame.</param>
        /// <param name="texture">The underlying texture.</param>
        public SpriteFrame(string name, Texture texture)
            : base(name, texture)
        {
            this.elements = new List<IElement>();
        }

        /// <inheritdoc />
        public List<IElement> Elements
        {
            get { return this.elements; }
            private set { this.elements = value; }
        }

        /// <inheritdoc />
        public float Scale
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
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
            base.Update();

            foreach (IElement element in this.elements)
            {
                element.Update();
            }
        }
    }
}
