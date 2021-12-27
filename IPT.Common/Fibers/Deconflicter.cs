using System.Collections.Generic;
using IPT.Common.User.Inputs;

namespace IPT.Common.Fibers
{
    public static class Deconflicter
    {
        private static HashSet<ButtonCombo> _buttonCombos = new HashSet<ButtonCombo>();
        private static HashSet<KeyCombo> _keyCombos = new HashSet<KeyCombo>();

        public static bool Add(GenericCombo combo)
        {
            if (combo is KeyCombo keyCombo)
            {
                return _keyCombos.Add(keyCombo);
            }
            else if (combo is ButtonCombo buttonCombo)
            {
                return _buttonCombos.Add(buttonCombo);
            }

            return false;
        }

        public static bool Remove(GenericCombo combo)
        {
            if (combo is KeyCombo keyCombo)
            {
                return _keyCombos.Remove(keyCombo);
            }
            else if (combo is ButtonCombo buttonCombo)
            {
                return _buttonCombos.Remove(buttonCombo);
            }

            return false;
        }
    }
}
