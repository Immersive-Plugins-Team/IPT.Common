﻿using System.Collections.Generic;
using System.Drawing;
using Rage;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// A frame containing multiple sprites.
    /// </summary>
    /// <typeparam name="T">The type of the sprites that the frame contains.</typeparam>
    public class Frame<T> : TextureElement, IContainer, IInteractive
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

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public bool IsHovered { get; set; } = false;

        /// <inheritdoc />
        public bool IsPressed { get; set; } = false;

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
                item.Parent = this;
                this.Items.Add(item);
            }
            else
            {
                throw new System.ArgumentException("Frame only allows elements of type IElement or its subclasses.", "item");
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
            // todo -- this isn't right, look at Refresh in TextureFrame.cs under GUI
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