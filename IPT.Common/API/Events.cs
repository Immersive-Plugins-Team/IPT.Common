using System;
using IPT.Common.User.Inputs;

namespace IPT.Common.API
{
    /// <summary>
    /// Events raised or consumed.
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Delegate event handler for user input changes.
        /// </summary>
        /// <param name="combo">The key or controller combo that has changed.</param>
        public delegate void UserInputChangedEventHandler(GenericCombo combo);

        /// <summary>
        /// Delegate event handler for holdable user inputs.
        /// </summary>
        /// <param name="combo">The holdable key or controller combo.</param>
        /// <param name="isLongPress">True if it was a long press, otherwise false.</param>
        public delegate void HoldableUserInputEventHandler(HoldableCombo combo, bool isLongPress);

        /// <summary>
        /// Event for user input changes.
        /// </summary>
        public static event UserInputChangedEventHandler OnUserInputChanged;

        /// <summary>
        ///  Event for holdable user inputs.
        /// </summary>
        public static event HoldableUserInputEventHandler OnHoldableUserInput;

        /// <summary>
        /// Fires an event for a user input change.
        /// </summary>
        /// <param name="combo">The key or controller combo that has changed.</param>
        internal static void FireUserInputChanged(GenericCombo combo)
        {
            OnUserInputChanged?.Invoke(combo);
        }

        /// <summary>
        /// Fires an event for a holdable user input.
        /// </summary>
        /// <param name="combo">The key or controller combo.</param>
        /// <param name="isLongPress">True if it was a long press, otherwise false.</param>
        internal static void FireHoldableUserInput(HoldableCombo combo, bool isLongPress)
        {
            OnHoldableUserInput?.Invoke(combo, isLongPress);
        }
    }
}
