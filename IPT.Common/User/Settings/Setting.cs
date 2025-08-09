using System;
using Rage;

namespace IPT.Common.User.Settings
{
    /// <summary>
    /// Abstract class that represents a user setting.
    /// </summary>
    public abstract class Setting
    {
        /// <summary>
        /// An event that is triggered when the value of the setting changes.
        /// </summary>
        public event Action<Setting> OnValueChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        /// <param name="section">The section of the INI file containing the setting.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="description">A brief description of the setting.</param>
        protected Setting(string section, string name, string description)
        {
            Section = section;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets the name of the setting.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the section of the INI file containing the setting.
        /// </summary>
        public string Section { get; private set; }

        /// <summary>
        /// Gets a brief description of the setting.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the value of the setting.
        /// </summary>
        /// <returns>The value.</returns>
        public abstract object GetValue();

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract void SetValue(object value);

        /// <summary>
        /// Loads the value.
        /// </summary>
        /// <param name="ini">The INI object used to load the value.</param>
        public abstract void Load(InitializationFile ini);

        /// <summary>
        /// Saves the value.
        /// </summary>
        /// <param name="ini">The INI object used to save the value.</param>
        public abstract void Save(InitializationFile ini);

        /// <summary>
        /// Fires the OnValueChanged event. To be called by subclasses.
        /// </summary>
        /// <param name="newValue">The new value being assigned.</param>
        protected void ValueChanged()
        {
            OnValueChanged?.Invoke(this);
        }
    }
}
