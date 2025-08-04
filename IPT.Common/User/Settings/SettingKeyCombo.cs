using System.Windows.Forms;
using IPT.Common.User.Inputs;
using Rage;

namespace IPT.Common.User.Settings
{
    /// <summary>
    /// A key combination based user setting from an INI file.
    /// </summary>
    public class SettingKeyCombo : Setting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingKeyCombo"/> class.
        /// </summary>
        /// <param name="section">The section of the INI file containing the setting.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="description">A brief description of the setting.</param>
        public SettingKeyCombo(string section, string name, string description)
            : base(section, name, description)
        {
            Value = new KeyCombo(Keys.None, Keys.None);
        }

        /// <summary>
        /// Gets the value of the setting.
        /// </summary>
        public KeyCombo Value { get; private set; }

        /// <summary>
        /// Gets the value of the setting.
        /// </summary>
        /// <returns>The value of the setting.</returns>
        public override object GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The value of the setting.</param>
        public override void SetValue(object value)
        {
            var newValue = (KeyCombo)value;
            if (Value == newValue) return;
            Value = newValue;
            ValueChanged(Value);
        }

        /// <summary>
        /// Loads the value.
        /// </summary>
        /// <param name="ini">The INI object used to load the value.</param>
        public override void Load(InitializationFile ini)
        {
            Keys primary = GetKeysFromString(ini.ReadString(Section, Name, Value.PrimaryKey.ToString()), Value.PrimaryKey);
            Keys secondary = GetKeysFromString(ini.ReadString(Section, $"{Name}Modifier", Value.SecondaryKey.ToString()), Value.SecondaryKey);
            Value = new KeyCombo(primary, secondary);
        }

        /// <summary>
        /// Saves the value.
        /// </summary>
        /// <param name="ini">The INI object used to save the value.</param>
        public override void Save(InitializationFile ini)
        {
            ini.Write(Section, Name, Value.PrimaryKey.ToString());
            if (Value.HasSecondary)
            {
                ini.Write(Section, $"{Name}Modifier", Value.SecondaryKey.ToString());
            }
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
