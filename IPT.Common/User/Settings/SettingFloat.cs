using IPT.Common.API;
using Rage;

namespace IPT.Common.User.Settings
{
    /// <summary>
    /// A float-based user setting.
    /// </summary>
    public class SettingFloat : Setting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingFloat"/> class.
        /// </summary>
        /// <param name="section">The section of the INI file containing the setting.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="description">A brief description of the setting.</param>
        /// <param name="defaultValue">The default value of the setting.</param>
        /// <param name="min">The minimum value of the setting.</param>
        /// <param name="max">The maximum value of the setting.</param>
        /// <param name="increment">The increment used to snap the setting.</param>
        public SettingFloat(string section, string name, string description, float defaultValue, float min, float max, float increment)
            : base(section, name, description)
        {
            Value = defaultValue;
            Min = min;
            Max = max;
            Increment = increment;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public float Min { get; private set; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public float Max { get; private set; }

        /// <summary>
        /// Gets the increment used for snapping.
        /// </summary>
        public float Increment { get; private set; }

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
            var newValue = Math.Snap((float)value, Min, Max, Increment);
            if (Value == newValue) return;
            Value = newValue;
            ValueChanged();
        }

        /// <summary>
        /// Loads the value.
        /// </summary>
        /// <param name="ini">The INI object used to load the value.</param>
        public override void Load(InitializationFile ini)
        {
            Value = System.Convert.ToSingle(ini.ReadDouble(Section, Name, Value));
            Value = Math.Snap(Value, Min, Max, Increment);
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
