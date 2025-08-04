using System;
using System.Collections.Generic;
using System.Net;
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

        private readonly List<GenericCombo> _combos = new List<GenericCombo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InputHandler"/> class.
        /// </summary>
        /// <param name="config">The configuration object for the plugin.</param>
        public InputHandler(Configuration config)
            : base("InputHandler", 0)
        {
            _combos.AddRange(config.GetInputCombos());
        }

        /// <summary>
        /// Executes each tick.
        /// </summary>
        protected override void DoSomething()
        {
            if (API.Functions.IsGamePaused()) return;
            foreach (var combo in _combos)
            {
                var state = combo.Check();
                switch (state)
                {
                    case InputState.Pressed: OnInputPressed?.Invoke(combo); break;
                    case InputState.Released: OnInputReleased?.Invoke(combo); break;
                    case InputState.LongPress: OnHoldableInput?.Invoke(combo as HoldableCombo, true); break;
                    case InputState.ShortPress: OnHoldableInput?.Invoke(combo as HoldableCombo, false); break;
                }
            }
        }
    }
}
