using Rage;

namespace IPT.Common.User
{
    public class SettingString : Setting
    {
        public SettingString(string section, string name, string description, string defaultValue)
            : base(section, name, description)
        {
            this.Value = defaultValue;
        }

        public string Value { get; private set; }

        public override object GetValue()
        {
            return this.Value;
        }

        public override void SetValue(object value)
        {
            this.Value = value.ToString();
        }

        public override void Load(InitializationFile ini)
        {
            this.Value = ini.ReadString(this.Section, this.Name, this.Value);
        }

        public override void Save(InitializationFile ini)
        {
            ini.Write(this.Section, this.Name, this.Value);
        }
    }
}
