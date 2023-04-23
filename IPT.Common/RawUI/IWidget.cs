using System.Drawing;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents an element that can be moved and resized.
    /// </summary>
    public interface IWidget : IDrawable
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
        /// Gets or sets a value indicating whether the element is enabled and can respond to user interaction.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the element is currently being hovered over by the mouse cursor.
        /// </summary>
        bool IsHovered { get; set; }

        /// <summary>
        /// Gets a value indicating whether the element is currently being resized.
        /// </summary>
        bool IsResizing { get; }

        /// <summary>
        /// Gets the offset between the mouse cursor and the corner of the element being used for the resize.
        /// </summary>
        PointF ResizeOffset { get; }

        /// <summary>
        /// Called when the element is being dragged.
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        void Drag(PointF mousePosition);

        /// <summary>
        /// Called when the element stops being dragged.
        /// </summary>
        void EndDrag();

        /// <summary>
        /// Called when the element stops being resized.
        /// </summary>
        void EndResize();

        /// <summary>
        /// Called when the element is being resized.
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        void Resize(PointF mousePosition);

        /// <summary>
        /// Called when the element starts being dragged.
        /// </summary>
        /// <param name="mousePosition">The position of the mouse cursor when the drag started.</param>
        void StartDrag(PointF mousePosition);

        /// <summary>
        /// Called when the element starts being resized.
        /// </summary>
        /// <param name="mousePosition">The position of the mouse cursor when the resize started.</param>
        void StartResize(PointF mousePosition);
    }
}
