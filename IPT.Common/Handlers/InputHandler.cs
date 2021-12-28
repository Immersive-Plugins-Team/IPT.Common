using System.Collections.Generic;
using IPT.Common.Fibers;
using IPT.Common.User;

namespace IPT.Common.Handlers
{
    /// <summary>
    /// A handler for managing user input.
    /// </summary>
    public class InputHandler
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
        /// Gets a list of fibers managed by the handler.
        /// </summary>
        public List<GenericFiber> Fibers
        {
            get
            {
                return this._fibers;
            }
        }

        /// <summary>
        /// Starts the input handler.
        /// </summary>
        /// <returns>The number of fibers started.</returns>
        public int Start()
        {
            foreach (var combo in this._config.GetInputCombos())
            {
                var fiber = new ComboFiber(combo);
                fiber.Start();
                this._fibers.Add(fiber);
            }

            return this._fibers.Count;
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
