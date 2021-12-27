using System;
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
            : base(0)
        {
            this.Combo = combo;
        }

        /// <summary>
        /// Gets the key or button combination being monitored.
        /// </summary>
        protected GenericCombo Combo { get; }

        /// <summary>
        /// Starts the Combo monitoring fiber.
        /// </summary>
        protected override void Start()
        {
            if (this.IsRunning)
            {
                return;
            }

            if (Deconflicter.Add(this.Combo))
            {
                GameFiber.StartNew(this.Run, $"COMBO-{this.Combo.ToString().ToUpper()}-{Guid.NewGuid()}");
            }
        }
    }
}
