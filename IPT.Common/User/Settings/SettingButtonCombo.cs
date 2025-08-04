using IPT.Common.User.Inputs;
using Rage;

namespace IPT.Common.User.Settings
{
    /// <summary>
    ///  A button combination based user setting from an INI file.
    /// </summary>
    public class SettingButtonCombo : Setting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingButtonCombo"/> class.
        /// </summary>
        /// <param name="section">The section of the INI file containing the setting.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="description">A brief description of the setting.</param>
        public SettingButtonCombo(string section, string name, string description)
            : base(section, name, description)
        {
            Value = new ButtonCombo(ControllerButtons.None, ControllerButtons.None);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public ButtonCombo Value { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        public override object GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue(object value)
        {
            var newValue = (ButtonCombo)value;
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
            ControllerButtons primary = ini.ReadEnum(Section, Name, Value.PrimaryButton);
            ControllerButtons secondary = ini.ReadEnum(Section, $"{Name}Modifier", Value.SecondaryButton);
            Value = new ButtonCombo(primary, secondary);
        }

        /// <summary>
        /// Saves the value.
        /// </summary>
        /// <param name="ini">The INI object used to save the value.</param>
        public override void Save(InitializationFile ini)
        {
            ini.Write(Section, Name, Value.PrimaryButton.ToString());
            if (Value.HasSecondary)
            {
                ini.Write(Section, $"{Name}Modifier", Value.SecondaryButton.ToString());
            }
        }
    }
}
