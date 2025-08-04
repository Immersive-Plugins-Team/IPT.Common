using IPT.Common.API;
using Rage;

namespace IPT.Common.User.Settings
{
    /// <summary>
    /// An int-based user setting from an INI file.
    /// </summary>
    public class SettingInt : Setting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingInt"/> class.
        /// </summary>
        /// <param name="section">The section of the INI file containing the setting.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="description">A brief description of the setting.</param>
        /// <param name="defaultValue">The default value of the setting.</param>
        /// <param name="min">The minimum (inclusive) valid value.</param>
        /// <param name="max">The maximum (inclusive) valid value.</param>
        /// <param name="increment">Used for snapping values to a set increment.</param>
        public SettingInt(string section, string name, string description, int defaultValue, int min, int max, int increment)
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
        public int Value { get; private set; }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public int Min { get; private set; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public int Max { get; private set; }

        /// <summary>
        /// Gets the increment used for snapping.
        /// </summary>
        public int Increment { get; private set; }

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
            var newValue = Math.Snap((int)value, Min, Max, Increment);
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
            Value = ini.ReadInt32(Section, Name, Value);
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
