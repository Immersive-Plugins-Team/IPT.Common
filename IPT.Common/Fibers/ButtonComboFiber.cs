using IPT.Common.User.Inputs;

namespace IPT.Common.Fibers
{
    /// <summary>
    /// A fiber for monitoring controller button input changes.
    /// </summary>
    public class ButtonComboFiber : ComboFiber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonComboFiber"/> class.
        /// </summary>
        /// <param name="combo">The ButtonCombo object to monitor.</param>
        public ButtonComboFiber(ButtonCombo combo)
            : base(combo)
        {
        }

        /// <summary>
        /// Gets the underlying ButtonCombo object monitored by this fiber.
        /// </summary>
        public ButtonCombo ButtonCombo
        {
            get
            {
                if (this.Combo is ButtonCombo buttonCombo)
                {
                    return buttonCombo;
                }

                return null;
            }
        }
    }
}
