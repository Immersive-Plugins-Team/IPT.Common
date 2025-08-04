using System;
using System.Collections.Generic;
using IPT.Common.Fibers;
using IPT.Common.User;
using IPT.Common.User.Inputs;
using Rage;

namespace IPT.Common.Handlers
{
    /// <summary>
    /// A handler for managing user input.
    /// </summary>
    public class InputHandler : GenericFiber
    {
        public event Action<GenericCombo> OnInputPressed;
        public event Action<GenericCombo> OnInputReleased;
        public event Action<HoldableCombo, bool> OnHoldableInput;

        private readonly Configuration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputHandler"/> class.
        /// </summary>
        /// <param name="config">The configuration object for the plugin.</param>
        public InputHandler(Configuration config)
            : base("InputHandler", 0)
        {
            _config = config;
        }

        /// <summary>
        /// Executes each tick.
        /// </summary>
        protected override void DoSomething()
        {
            if (API.Functions.IsGamePaused()) return;

            // we have to retrieve the actual combos every time because
            // originally I was using reference comparisons instead of
            // value comparisons because I'm a fucking idiot
            foreach (var combo in _config.GetInputCombos())
            {
                if (combo is HoldableCombo holdable)
                {
                    switch (holdable.Check())
                    {
                        case InputState.ShortPress:
                            OnHoldableInput?.Invoke(holdable, false);
                            API.Events.FireHoldableUserInput(holdable, false); // legacy
                            break;
                        case InputState.LongPress:
                            OnHoldableInput?.Invoke(holdable, true);
                            API.Events.FireHoldableUserInput(holdable, true);  // legacy
                            break;
                    }

                }
                else
                {
                    switch (combo.Check())
                    {
                        case InputState.Pressed:
                            OnInputPressed?.Invoke(combo);
                            API.Events.FireUserInputChanged(combo);
                            break;
                        case InputState.Released:
                            OnInputReleased?.Invoke(combo);
                            API.Events.FireUserInputChanged(combo);
                            break;
                    }
                }
            }
        }
    }
}
