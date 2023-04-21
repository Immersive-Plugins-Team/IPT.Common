using System.Collections.Generic;

namespace IPT.Common.RUI
{
    /// <summary>
    /// Represents a container that can contain multiple renderable elements.
    /// </summary>
    public interface IRenderableContainer : IRenderable
    {
        /// <summary>
        /// Gets the list of child elements contained within the container.
        /// </summary>
        List<IRenderable> Elements { get; }

        /// <summary>
        /// Gets or sets the scaling factor to be applied to the container and its child elements.
        /// </summary>
        float Scale { get; set; }

        /// <summary>
        /// Adds an <see cref="IRenderable"/> child element to the container.
        /// </summary>
        /// <param name="element">The child element to add.</param>
        void AddElement(IRenderable element);

        /// <summary>
        /// Removes an <see cref="IRenderable"/> child element from the container.
        /// </summary>
        /// <param name="element">The child element to remove.</param>
        void RemoveElement(IRenderable element);

        /// <summary>
        /// Removes all child elements from the container.
        /// </summary>
        void ClearElements();
    }
}
