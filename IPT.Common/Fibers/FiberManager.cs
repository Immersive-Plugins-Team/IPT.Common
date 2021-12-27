using System.Collections.Generic;
using IPT.Common.User.Inputs;

namespace IPT.Common.Fibers
{
    /// <summary>
    /// Manages the starting and stopping of fibers to prevent conflicts.
    /// This is the preferred way of starting and stopping fibers.
    /// </summary>
    public static class FiberManager
    {
        private static HashSet<ButtonCombo> _buttonCombos = new HashSet<ButtonCombo>();
        private static HashSet<KeyCombo> _keyCombos = new HashSet<KeyCombo>();
        private static HashSet<GenericFiber> _fibers = new HashSet<GenericFiber>();

        /// <summary>
        /// Starts a fiber.
        /// </summary>
        /// <param name="fiber">The fiber to start.</param>
        public static void Start(GenericFiber fiber)
        {
            _fibers.Add(fiber);
            if (fiber.IsRunning)
            {
                return;
            }

            if (fiber is KeyComboFiber keyComboFiber)
            {
                if (_keyCombos.Add(keyComboFiber.KeyCombo))
                {
                    keyComboFiber.Start();
                    _fibers.Add(fiber);
                }
            }
            else if (fiber is ButtonComboFiber buttonComboFiber)
            {
                if (_buttonCombos.Add(buttonComboFiber.ButtonCombo))
                {
                    buttonComboFiber.Start();
                    _fibers.Add(fiber);
                }
            }
            else
            {
                fiber.Start();
            }
        }

        /// <summary>
        /// Stops a fiber.
        /// </summary>
        /// <param name="fiber">The fiber to stop.</param>
        public static void Stop(GenericFiber fiber)
        {
            _fibers.Remove(fiber);
            if (!fiber.IsRunning)
            {
                return;
            }

            fiber.Stop();
            if (fiber is KeyComboFiber keyComboFiber)
            {
                _keyCombos.Remove(keyComboFiber.KeyCombo);
            }
            else if (fiber is ButtonComboFiber buttonComboFiber)
            {
                _buttonCombos.Remove(buttonComboFiber.ButtonCombo);
            }
        }
    }
}
