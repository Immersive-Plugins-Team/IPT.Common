using IPT.Common.API;

namespace IPT.Common.User.Inputs
{
    /// <summary>
    /// A generic class for defining combinations of keys or controller buttons.
    /// </summary>
    public abstract class GenericCombo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCombo"/> class.
        /// </summary>
        /// <param name="primary">The primary key or button.</param>
        /// <param name="secondary">The optional secondary key or button.</param>
        protected GenericCombo(object primary, object secondary)
        {
            this.Primary = primary;
            this.Secondary = secondary;
            this.IsPressed = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the key or button is currently marked as being pressed.
        /// </summary>
        public bool IsPressed { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether or not the combination has a secondary key or button.
        /// </summary>
        public abstract bool HasSecondary { get; }

        /// <summary>
        /// Gets the primary key or button.
        /// </summary>
        protected object Primary { get; }

        /// <summary>
        /// Gets the secondary key or button.  Optional.
        /// </summary>
        protected object Secondary { get; }

        /// <summary>
        /// Checks the actual status of the key/button combo and updates if it has changed.
        /// </summary>
        public virtual void Check()
        {
            if (this.CheckGameIsPressed() != this.IsPressed)
            {
                this.IsPressed = !this.IsPressed;
                Events.FireUserInputChanged(this);
            }
        }

        /// <summary>
        /// Returns a string representation of the combination.
        /// </summary>
        /// <returns>A string that represents the combination.</returns>
        public override string ToString()
        {
            return (this.HasSecondary ? $"{this.Secondary}+" : string.Empty) + this.Primary.ToString();
        }

        /// <summary>
        /// Internally check to see if the controller or keyboard is currently pressed.
        /// </summary>
        /// <returns>Returns true if the key/button combo is currently pressed.</returns>
        protected abstract bool CheckGameIsPressed();
    }
}
