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
        private readonly Configuration config;
        private readonly List<GenericFiber> fibers;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputHandler"/> class.
        /// </summary>
        /// <param name="config">The configuration object for the plugin.</param>
        public InputHandler(Configuration config)
        {
            this.config = config;
            this.fibers = new List<GenericFiber>();
        }

        /// <summary>
        /// Gets a list of fibers managed by the handler.
        /// </summary>
        public List<GenericFiber> Fibers
        {
            get
            {
                return this.fibers;
            }
        }

        /// <summary>
        /// Starts the input handler.
        /// </summary>
        public void Start()
        {
            foreach (var combo in this.config.GetInputCombos())
            {
                var fiber = new ComboFiber(combo);
                fiber.Start();
                this.fibers.Add(fiber);
            }
        }

        /// <summary>
        /// Stops the input handler.
        /// </summary>
        public void Stop()
        {
            foreach (var fiber in this.fibers)
            {
                fiber.Stop();
            }

            this.fibers.Clear();
        }
    }
}
