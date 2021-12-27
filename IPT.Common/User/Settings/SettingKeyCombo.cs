using System.Windows.Forms;
using Rage;

namespace IPT.Common.User
{
    public class SettingKeyCombo : Setting
    {
        public SettingKeyCombo(string section, string name, string description)
            : base(section, name, description)
        {
            this.Value = new KeyCombo(Keys.None, Keys.None);
        }

        public KeyCombo Value { get; private set; }

        public override object GetValue()
        {
            return this.Value;
        }

        public override void SetValue(object value)
        {
            this.Value = (KeyCombo)value;
        }

        public override void Load(InitializationFile ini)
        {
            Keys primary = GetKeysFromString(ini.ReadString(this.Section, this.Name, this.Value.PrimaryKey.ToString()), this.Value.PrimaryKey);
            Keys secondary = GetKeysFromString(ini.ReadString(this.Section, $"{this.Name}Modifier", this.Value.SecondaryKey.ToString()), this.Value.SecondaryKey);
            this.Value = new KeyCombo(primary, secondary);
        }

        public override void Save(InitializationFile ini)
        {
            ini.Write(this.Section, this.Name, this.Value);
        }

        private static Keys GetKeysFromString(string keyString, Keys defaultKeys)
        {
            try
            {
                return (Keys)new KeysConverter().ConvertFromString(keyString);
            }
            catch
            {
                return defaultKeys;
            }
        }
    }
}
