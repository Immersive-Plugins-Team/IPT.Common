using System.Collections.Generic;

namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a container that can contain items.
    /// </summary>
    /// <typeparam name="T">The type of items in the container.</typeparam>
    public interface IContainer<T> : IParent, IDrawable
        where T : IDrawable
    {
        /// <summary>
        /// Gets the list of items contained within the container.
        /// </summary>
        List<T> Items { get; }

        /// <summary>
        /// Adds an item to the container.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(T item);

        /// <summary>
        /// Removes an item from the container.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        void Remove(T item);

        /// <summary>
        /// Removes all items the container.
        /// </summary>
        void Clear();
    }
}
