using Rage;

namespace IPT.Common.User.Settings
{
    /// <summary>
    /// A string-based user setting from an INI file.
    /// </summary>
    public class SettingString : Setting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingString"/> class.
        /// </summary>
        /// <param name="section">The INI section containing the setting.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="description">A brief description of the setting.</param>
        /// <param name="defaultValue">The default value to use for the setting.</param>
        public SettingString(string section, string name, string description, string defaultValue)
            : base(section, name, description)
        {
            Value = defaultValue;
        }

        /// <summary>
        /// Gets the value of the setting.
        /// </summary>
        public string Value { get; private set; }

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
        /// <param name="value">The value to use.</param>
        public override void SetValue(object value)
        {
            var newValue = value.ToString();
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
            Value = ini.ReadString(Section, Name, Value);
        }

        /// <summary>
        /// Saves the value.
        /// </summary>
        /// <param name="ini">The INI object used to save the value.</param>
        public override void Save(InitializationFile ini)
        {
            ini.Write(Section, Name, Value);
        }
    }
}
