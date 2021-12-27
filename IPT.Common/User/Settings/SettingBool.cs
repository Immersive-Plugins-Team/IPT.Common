using Rage;

namespace IPT.Common.User
{
    public class SettingBool : Setting
    {
        public SettingBool(string section, string name, string description, bool defaultValue)
            : base(section, name, description)
        {
            this.Value = defaultValue;
        }

        public bool Value { get; private set; }

        public override object GetValue()
        {
            return this.Value;
        }

        public override void SetValue(object value)
        {
            this.Value = (bool)value;
        }

        public override void Load(InitializationFile ini)
        {
            this.Value = ini.ReadBoolean(this.Section, this.Name, this.Value);
        }

        public override void Save(InitializationFile ini)
        {
            ini.Write(this.Section, this.Name, this.Value);
        }
    }
}
