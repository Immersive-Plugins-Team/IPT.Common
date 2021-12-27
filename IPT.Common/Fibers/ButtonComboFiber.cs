using IPT.Common.User.Inputs;
using Rage;

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

        /// <summary>
        /// Checks every tick to see if the button status has changed.
        /// </summary>
        protected override void DoSomething()
        {
            if (!Game.IsPaused && !Rage.Native.NativeFunction.Natives.IS_PAUSE_MENU_ACTIVE<bool>())
            {
                this.Combo.Check();
            }
        }
    }
}
