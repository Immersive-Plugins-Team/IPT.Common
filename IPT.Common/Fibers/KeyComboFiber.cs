using IPT.Common.User.Inputs;

namespace IPT.Common.Fibers
{
    /// <summary>
    /// A fiber for monitoring key changes.
    /// </summary>
    public class KeyComboFiber : ComboFiber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyComboFiber"/> class.
        /// </summary>
        /// <param name="combo">The key combination to monitor.</param>
        public KeyComboFiber(KeyCombo combo)
            : base(combo)
        {
        }

        /// <summary>
        /// Gets a value for the key combination.
        /// </summary>
        public KeyCombo KeyCombo
        {
            get
            {
                if (this.Combo is KeyCombo keyCombo)
                {
                    return keyCombo;
                }

                return null;
            }
        }
    }
}
