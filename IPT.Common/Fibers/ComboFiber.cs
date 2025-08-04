using System;
using IPT.Common.User.Inputs;
using Rage;

namespace IPT.Common.Fibers
{
    /// <summary>
    /// A fiber for monitoring keyboard and controller inputs.
    /// </summary>
    [Obsolete("This class is deprecated. Use input handler.")]
    public class ComboFiber : GenericFiber
    {
        private readonly GenericCombo _combo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboFiber"/> class.
        /// </summary>
        /// <param name="combo">The key or button combination being monitored.</param>
        public ComboFiber(GenericCombo combo)
            : base($"combo-{combo}", 0)
        {
            _combo = combo;
        }

        /// <summary>
        /// Gets the key or button combination being monitored.
        /// </summary>
        public GenericCombo Combo => _combo;

        /// <summary>
        /// Checks every tick to see if the combo has changed its status.
        /// </summary>
        protected override void DoSomething()
        {
            if (API.Functions.IsGamePaused()) return;
            if (Combo is HoldableCombo holdable)
            {
                var holdableState = holdable.Check();
                if (holdableState != InputState.None) API.Events.FireHoldableUserInput(holdable, holdableState == InputState.LongPress);
            }
            else
            {
                var state = Combo.Check();
                if (state != InputState.None) API.Events.FireUserInputChanged(Combo);
            }
        }
    }
}
