using Rage;

namespace IPT.Common.User
{
    public class SettingButtonCombo : Setting
    {
        public SettingButtonCombo(string section, string name, string description)
            : base(section, name, description)
        {
            this.Value = new ButtonCombo(ControllerButtons.None, ControllerButtons.None);
        }

        public ButtonCombo Value { get; private set; }

        public override object GetValue()
        {
            return this.Value;
        }

        public override void SetValue(object value)
        {
            this.Value = (ButtonCombo)value;
        }

        public override void Load(InitializationFile ini)
        {
            ControllerButtons primary = ini.ReadEnum(this.Section, this.Name, this.Value.PrimaryButton);
            ControllerButtons secondary = ini.ReadEnum(this.Section, $"{this.Name}Modifier", this.Value.SecondaryButton);
            this.Value = new ButtonCombo(primary, secondary);
        }

        public override void Save(InitializationFile ini)
        {
            ini.Write(this.Section, this.Name, this.Value.PrimaryButton.ToString());
            ini.Write(this.Section, $"{this.Name}Modifier", this.Value.SecondaryButton.ToString());
        }
    }
}
