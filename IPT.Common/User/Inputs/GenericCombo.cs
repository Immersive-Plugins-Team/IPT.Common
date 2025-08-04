
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
            Primary = primary;
            Secondary = secondary;
            IsPressed = false;
        }

        public override bool Equals(object obj)
        {
            if (obj is GenericCombo other) return ToString() == other.ToString();
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(GenericCombo left, GenericCombo right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(GenericCombo left, GenericCombo right)
        {
            return !(left == right);
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
        public virtual InputState Check()
        {
            if (CheckGameIsPressed() == IsPressed) return InputState.None;
            IsPressed = !IsPressed;
            return IsPressed ? InputState.Pressed : InputState.Released;
        }

        /// <summary>
        /// Returns a string representation of the combination.
        /// </summary>
        /// <returns>A string that represents the combination.</returns>
        public override string ToString()
        {
            return (HasSecondary ? $"{Secondary}+" : string.Empty) + Primary.ToString();
        }

        /// <summary>
        /// Internally check to see if the controller or keyboard is currently pressed.
        /// </summary>
        /// <returns>Returns true if the key/button combo is currently pressed.</returns>
        protected abstract bool CheckGameIsPressed();
    }
}
