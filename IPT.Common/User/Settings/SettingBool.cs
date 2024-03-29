﻿using Rage;

namespace IPT.Common.User.Settings
{
    /// <summary>
    /// A boolean-based user setting.
    /// </summary>
    public class SettingBool : Setting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingBool"/> class.
        /// </summary>
        /// <param name="section">The section of the INI file containing the setting.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="description">A brief description of the setting.</param>
        /// <param name="defaultValue">The default value of the setting.</param>
        public SettingBool(string section, string name, string description, bool defaultValue)
            : base(section, name, description)
        {
            this.Value = defaultValue;
        }

        /// <summary>
        /// Gets a value indicating whether the value is true or false.
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        public override object GetValue()
        {
            return this.Value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue(object value)
        {
            this.Value = (bool)value;
        }

        /// <summary>
        /// Loads the value.
        /// </summary>
        /// <param name="ini">The INI object used to load the value.</param>
        public override void Load(InitializationFile ini)
        {
            this.Value = ini.ReadBoolean(this.Section, this.Name, this.Value);
        }

        /// <summary>
        /// Saves the value.
        /// </summary>
        /// <param name="ini">The INI object used to save the value.</param>
        public override void Save(InitializationFile ini)
        {
            ini.Write(this.Section, this.Name, this.Value);
        }
    }
}
