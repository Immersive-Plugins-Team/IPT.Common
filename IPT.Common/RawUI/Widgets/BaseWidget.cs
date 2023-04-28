﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.API;
using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Widgets
{
    /// <summary>
    /// Represents a base widget.
    /// </summary>
    public abstract class BaseWidget : IObserver, IWidget
    {
        /// <inheritdoc/>
        public RectangleF Bounds { get; protected set; } = default;

        /// <inheritdoc/>
        public PointF DragOffset { get; protected set; } = new PointF(0, 0);

        /// <inheritdoc/>
        public int Height { get; protected set; }

        /// <inheritdoc/>
        public bool IsDragging { get; protected set; } = false;

        /// <inheritdoc/>
        public bool IsHovered { get; set; } = false;

        /// <inheritdoc/>
        public bool IsVisible { get; set; } = true;

        /// <inheritdoc/>
        public List<IDrawable> Items { get; protected set; } = new List<IDrawable>();

        /// <inheritdoc/>
        public IParent Parent { get; set; } = null;

        /// <inheritdoc/>
        public Point Position { get; protected set; } = default;

        /// <inheritdoc/>
        public SizeF Scale
        {
            get
            {
                if (this.Parent != null)
                {
                    return new SizeF(this.Parent.Scale.Width * this.WidgetScale, this.Parent.Scale.Height * this.WidgetScale);
                }

                return default;
            }
        }

        /// <inheritdoc/>
        public string UUID
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.UUID;
                }

                return System.Guid.Empty.ToString();
            }
        }

        /// <inheritdoc/>
        public float WidgetScale { get; protected set; } = 1f;

        /// <inheritdoc/>
        public int Width { get; protected set; }

        /// <inheritdoc/>
        public virtual void Add(IDrawable item)
        {
            item.Parent = this;
            this.Items.Add(item);
            if (item is IObservable observable)
            {
                observable.AddObserver(this);
            }
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
            this.Items.Clear();
        }

        /// <inheritdoc/>
        public virtual bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        /// <inheritdoc/>
        public void Drag(PointF mousePosition)
        {
            this.MoveTo(new Point((int)System.Math.Round(mousePosition.X + this.DragOffset.X), (int)System.Math.Round(mousePosition.Y + this.DragOffset.Y)));
        }

        /// <inheritdoc/>
        public virtual void Draw(Rage.Graphics g)
        {
            this.Items.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(g));
        }

        /// <inheritdoc/>
        public void MoveTo(Point position)
        {
            this.Position = position;
            this.UpdateBounds();
        }

        /// <inheritdoc />
        public virtual void OnUpdated(IObservable obj)
        {
        }

        /// <inheritdoc/>
        public virtual void Remove(IDrawable item)
        {
            this.Items.Remove(item);
        }

        /// <inheritdoc/>
        public void SetWidgetScale(float scale)
        {
            if (this.Parent is Canvas)
            {
                this.WidgetScale = API.Math.Clamp(scale, Constants.MinScale, Constants.MaxScale);
                this.UpdateBounds();
            }
        }

        /// <inheritdoc/>
        public void StartDrag(PointF mousePosition)
        {
            this.IsDragging = true;
            this.DragOffset = new PointF(this.Position.X - mousePosition.X, this.Position.Y - mousePosition.Y);
        }

        /// <inheritdoc/>
        public void StopDrag()
        {
            this.IsDragging = false;
            this.DragOffset = default;
        }

        /// <inheritdoc/>
        public virtual void UpdateBounds()
        {
            if (this.Parent == null)
            {
                Logging.Warning("cannot update bounds on parentless widget");
                return;
            }

            if (this.Parent is Canvas canvas)
            {
                float x = this.Position.X * canvas.Scale.Width;
                float y = this.Position.Y * canvas.Scale.Height;
                var size = new SizeF(this.Width * this.Scale.Height, this.Height * this.Scale.Height);
                this.Bounds = new RectangleF(new PointF(x, y), size);
            }
            else
            {
                float x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
                float y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
                var size = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
                this.Bounds = new RectangleF(new PointF(x, y), size);
            }

            foreach (IDrawable item in this.Items)
            {
                item.UpdateBounds();
            }
        }
    }
}
