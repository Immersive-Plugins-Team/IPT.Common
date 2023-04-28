using System.Collections.Generic;
using IPT.Common.RawUI.Interfaces;

namespace IPT.Common.RawUI.Elements
{
    /// <summary>
    /// Represents a clickable button.
    /// </summary>
    public class Button : TextureElement, IButton
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the button.</param>
        /// <param name="textureName">The texture name for the button.</param>
        public Button(string id, string textureName)
            : base(textureName, 0, 0)
        {
            this.Id = id;
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <inheritdoc/>
        public virtual bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Adds an observer.
        /// </summary>
        /// <param name="observer">The observer to add.</param>
        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <inheritdoc/>
        public virtual void Click()
        {
            foreach (var observer in this.observers)
            {
                observer.OnUpdated(this);
            }
        }

        /// <inheritdoc/>
        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        /// <summary>
        /// Removes an observer.
        /// </summary>
        /// <param name="observer">The observer to remove.</param>
        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }
    }
}
