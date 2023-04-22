using System.Drawing;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Interface for elements that can be dragged around.
    /// </summary>
    public interface IDraggable
    {
        /// <summary>
        /// Gets a value indicating whether the element is currently being dragged.
        /// </summary>
        bool IsDragging { get; }

        /// <summary>
        /// Gets the offset between the mouse cursor and the top-left corner of the element.
        /// </summary>
        Point DragOffset { get; }

        /// <summary>
        /// Called when the element starts being dragged.
        /// </summary>
        /// <param name="mousePosition">The position of the mouse cursor when the drag started.</param>
        void StartDrag(PointF mousePosition);

        /// <summary>
        /// Called when the element is being dragged.
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        void Drag(PointF mousePosition);

        /// <summary>
        /// Called when the element stops being dragged.
        /// </summary>
        void EndDrag();
    }
}
