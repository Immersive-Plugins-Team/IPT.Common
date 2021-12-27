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
        /// Even for user input changes.
        /// </summary>
        public static event UserInputChangedEventHandler OnUserInputChanged;

        /// <summary>
        /// Fires an event for a user input change.
        /// </summary>
        /// <param name="combo">The key or controller combo that has changed.</param>
        internal static void FireUserInputChanged(GenericCombo combo)
        {
            OnUserInputChanged?.Invoke(combo);
        }
    }
}
