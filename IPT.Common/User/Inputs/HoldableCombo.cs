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
            Interval = interval;
            Timer = new Stopwatch();
            IsLong = false;
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
        public override InputState Check()
        {
            if (CheckGameIsPressed() != IsPressed)
            {
                IsPressed = !IsPressed;
                if (IsPressed) Timer.Start();
                else
                {
                    Timer.Reset();
                    if (!IsLong) return InputState.ShortPress;
                    else IsLong = false; // Reset long press state on release
                }
            }
            else if (IsPressed && !IsLong && Timer.ElapsedMilliseconds > Interval)
            {
                IsLong = true;
                return InputState.LongPress;
            }

            return InputState.None;
        }
    }
}
