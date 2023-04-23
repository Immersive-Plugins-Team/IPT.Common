using System.Collections.Generic;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a container that can contain items.
    /// </summary>
    /// <typeparam name="T">The type of items held in the container.</typeparam>
    public interface IContainer : IDrawable
    {
        /// <summary>
        /// Gets the list of items contained within the container.
        /// </summary>
        List<IDrawable> Items { get; }

        /// <summary>
        /// Gets the scaling factor to be applied to the container and its items.
        /// </summary>
        float Scale { get; }

        /// <summary>
        /// Adds an item to the container.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(IDrawable item);

        /// <summary>
        /// Removes an item from the container.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        void Remove(IDrawable item);

        /// <summary>
        /// Removes all items the container.
        /// </summary>
        void Clear();
    }
}
