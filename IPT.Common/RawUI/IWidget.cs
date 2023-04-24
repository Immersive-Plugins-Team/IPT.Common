﻿using System.Drawing;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a container that can be moved and resized.
    /// </summary>
    public interface IWidget : IContainer, IControl
    {
        /// <summary>
        /// Gets the offset between the mouse cursor and the top-left corner of the element when being dragged.
        /// </summary>
        PointF DragOffset { get; }

        /// <summary>
        /// Gets a value indicating whether the element is currently being dragged.
        /// </summary>
        bool IsDragging { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the element is currently being hovered over by the mouse cursor.
        /// </summary>
        bool IsHovered { get; set; }

        /// <summary>
        /// Gets a value for the widget specific scale.
        /// </summary>
        float WidgetScale { get; }

        /// <summary>
        /// Called when the element is being dragged.
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        void Drag(PointF mousePosition);

        /// <summary>
        /// Safely sets the widget's scale.
        /// </summary>
        /// <param name="scale">The widget specific scale where 1.0 is native.</param>
        void SetWidgetScale(float scale);

        /// <summary>
        /// Called when the element starts being dragged.
        /// </summary>
        /// <param name="mousePosition">The position of the mouse cursor when the drag started.</param>
        void StartDrag(PointF mousePosition);

        /// <summary>
        /// Called when the element stops being dragged.
        /// </summary>
        void StopDrag();
    }
}
