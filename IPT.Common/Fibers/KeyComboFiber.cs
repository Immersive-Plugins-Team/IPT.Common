using Rage;

namespace IPT.Common.User
{
    public class KeyComboFiber : ComboFiber
    {
        public KeyComboFiber(KeyCombo combo)
            : base(combo)
        {
        }

        public KeyCombo KeyCombo
        {
            get
            {
                if (this.Combo is KeyCombo keyCombo)
                {
                    return keyCombo;
                }

                return null;
            }
        }

        protected override void Check()
        {
            if (!Game.IsPaused && !Rage.Native.NativeFunction.Natives.IS_PAUSE_MENU_ACTIVE<bool>())
            {
                var state = Game.GetKeyboardState();
                switch (this.KeyCombo.IsPressed)
                {
                    case true:
                        if (!state.PressedKeys.Contains(this.KeyCombo.PrimaryKey))
                        {
                            this.KeyCombo.Toggle();
                        }

                        break;
                    case false:
                        if (state.PressedKeys.Contains(this.KeyCombo.PrimaryKey))
                        {
                            switch (this.KeyCombo.HasSecondary)
                            {
                                case true:
                                    if (state.PressedKeys.Contains(this.KeyCombo.SecondaryKey))
                                    {
                                        this.KeyCombo.Toggle();
                                    }

                                    break;
                                case false:
                                    if (state.PressedKeys.Count == 1)
                                    {
                                        this.KeyCombo.Toggle();
                                    }

                                    break;
                            }
                        }

                        break;
                }
            }
        }
    }
}
