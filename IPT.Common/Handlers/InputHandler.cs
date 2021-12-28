using System.Collections.Generic;
using IPT.Common.API;
using IPT.Common.Fibers;
using IPT.Common.User;
using IPT.Common.User.Inputs;

namespace IPT.Common.Handlers
{
    /// <summary>
    /// A handler for managing user input.
    /// </summary>
    public class InputHandler : IHandler
    {
        private readonly Configuration _config;
        private readonly List<GenericFiber> _fibers;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputHandler"/> class.
        /// </summary>
        /// <param name="config">The configuration object for the plugin.</param>
        public InputHandler(Configuration config)
        {
            this._config = config;
            this._fibers = new List<GenericFiber>();
        }

        /// <summary>
        /// Starts the input handler.
        /// </summary>
        public void Start()
        {
            foreach (var combo in this._config.GetInputCombos())
            {
                if (combo is ButtonCombo buttonCombo)
                {
                    Logging.Debug($"starting new fiber for button combo: {buttonCombo}");
                    var fiber = new ButtonComboFiber(buttonCombo);
                    fiber.Start();
                    this._fibers.Add(fiber);
                }
                else if (combo is KeyCombo keyCombo)
                {
                    Logging.Debug($"starting new fiber for key combo: {keyCombo}");
                    var fiber = new KeyComboFiber(keyCombo);
                    fiber.Start();
                    this._fibers.Add(fiber);
                }
            }
        }

        /// <summary>
        /// Stops the input handler.
        /// </summary>
        public void Stop()
        {
            foreach (var fiber in this._fibers)
            {
                fiber.Stop();
            }

            this._fibers.Clear();
        }
    }
}
