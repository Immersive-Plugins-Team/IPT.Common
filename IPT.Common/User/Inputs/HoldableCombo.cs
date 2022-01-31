using System.Diagnostics;

namespace IPT.Common.User.Inputs
{
    /// <summary>
    /// A combo that can be either a short or long press.
    /// </summary>
    public abstract class HoldableCombo : GenericCombo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HoldableCombo"/> class.
        /// </summary>
        /// <param name="primary">The primary key or button.</param>
        /// <param name="secondary">The secondary key or button.</param>
        /// <param name="interval">How long to wait for a long press.</param>
        protected HoldableCombo(object primary, object secondary, int interval)
            : base(primary, secondary)
        {
            this.Interval = interval;
            this.Timer = new Stopwatch();
            this.IsLong = false;
        }

        /// <summary>
        /// Gets or sets a value indicating how long to wait for a long press.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not a long press was made.
        /// </summary>
        public bool IsLong { get; protected set; }

        /// <summary>
        /// Gets or sets the timer used for measuring long presses.
        /// </summary>
        public Stopwatch Timer { get; protected set; }

        /// <summary>
        /// Checks the status of the combo.
        /// </summary>
        public override void Check()
        {
            if (this.CheckGameIsPressed() != this.IsPressed)
            {
                this.IsPressed = !this.IsPressed;
                if (this.IsPressed)
                {
                    this.Timer.Start();
                }
                else
                {
                    this.Timer.Reset();
                    if (!this.IsLong)
                    {
                        API.Events.FireHoldableUserInput(this, false);
                    }
                    else
                    {
                        // we already fired the event from a long press
                        this.IsLong = false;
                    }
                }
            }
            else if (this.IsPressed && !this.IsLong && this.Timer.ElapsedMilliseconds > this.Interval)
            {
                this.IsLong = true;
                API.Events.FireHoldableUserInput(this, true);
            }
        }
    }
}
