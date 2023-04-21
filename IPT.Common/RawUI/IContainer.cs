using System.Collections.Generic;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a container that can contain items.
    /// </summary>
    public interface IContainer : IDrawable
    {
        /// <summary>
        /// Gets the list of the items contained within the container.
        /// </summary>
        List<IDrawable> Items { get; }

        /// <summary>
        /// Gets the scaling factor to be applied to the container and its items.
        /// </summary>
        float Scale { get; }

        /// <summary>
        /// Adds an <see cref="IDrawable"/> item to the container.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void AddItem(IDrawable item);

        /// <summary>
        /// Removes an <see cref="IDrawable"/> item from the container.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        void RemoveItem(IDrawable item);

        /// <summary>
        /// Removes all items the container.
        /// </summary>
        void ClearItems();
    }
}
