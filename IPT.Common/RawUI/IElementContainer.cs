using System.Collections.Generic;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a container that can contain multiple renderable elements.
    /// </summary>
    public interface IElementContainer : IElement
    {
        /// <summary>
        /// Gets the list of child elements contained within the container.
        /// </summary>
        List<IElement> Elements { get; }

        /// <summary>
        /// Gets the scaling factor to be applied to the container and its child elements.
        /// </summary>
        float Scale { get; }

        /// <summary>
        /// Adds an <see cref="IElement"/> child element to the container.
        /// </summary>
        /// <param name="element">The child element to add.</param>
        void AddElement(IElement element);

        /// <summary>
        /// Removes an <see cref="IElement"/> child element from the container.
        /// </summary>
        /// <param name="element">The child element to remove.</param>
        void RemoveElement(IElement element);

        /// <summary>
        /// Removes all child elements from the container.
        /// </summary>
        void ClearElements();
    }
}
