using System;
using System.Windows.Forms;
using Rage;

namespace IPT.Common.User.Inputs
{
    /// <summary>
    /// A combination of one or two keyboard keys.
    /// </summary>
    public sealed class KeyCombo : GenericCombo, IEquatable<KeyCombo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyCombo"/> class.
        /// </summary>
        /// <param name="primary">The primary keyboard key.</param>
        /// <param name="secondary">The optional secondary modifier key.</param>
        public KeyCombo(Keys primary, Keys secondary)
            : base(primary, secondary)
        {
        }

        /// <summary>
        /// Gets the value of the primary key.
        /// </summary>
        public Keys PrimaryKey
        {
            get
            {
                return (Keys)this.Primary;
            }
        }

        /// <summary>
        /// Gets a value of the optional modifier key.
        /// </summary>
        public Keys SecondaryKey
        {
            get
            {
                return (Keys)this.Secondary;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the key combination contains a secondary key.
        /// </summary>
        public override bool HasSecondary
        {
            get
            {
                return this.SecondaryKey != Keys.None;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the key combination is equal to another key combination.
        /// </summary>
        /// <param name="other">The key combination to compare.</param>
        /// <returns>True if the key combinations match.</returns>
        public bool Equals(KeyCombo other)
        {
            return this.PrimaryKey == other.PrimaryKey && this.SecondaryKey == other.SecondaryKey;
        }

        /// <summary>
        /// Checks the current state of the game to see if the key combination is pressed.
        /// </summary>
        /// <returns>True if the key combination is pressed.</returns>
        protected override bool CheckGameIsPressed()
        {
            var state = Game.GetKeyboardState();
            return state.PressedKeys.Contains(this.PrimaryKey) && (!this.HasSecondary || state.PressedKeys.Contains(this.SecondaryKey));
        }
    }
}
