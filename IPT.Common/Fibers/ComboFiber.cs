using IPT.Common.User.Inputs;
using Rage;

namespace IPT.Common.Fibers
{
    /// <summary>
    /// A fiber for monitoring keyboard and controller inputs.
    /// </summary>
    public abstract class ComboFiber : GenericFiber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboFiber"/> class.
        /// </summary>
        /// <param name="combo">The key or button combination being monitored.</param>
        protected ComboFiber(GenericCombo combo)
            : base($"{combo}-{combo}", 0)
        {
            this.Combo = combo;
        }

        /// <summary>
        /// Gets the key or button combination being monitored.
        /// </summary>
        protected GenericCombo Combo { get; }

        /// <summary>
        /// Checks every tick to see if the combo has changed its status.
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
