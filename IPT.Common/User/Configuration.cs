using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using IPT.Common.API;
using IPT.Common.User.Inputs;
using IPT.Common.User.Settings;
using Rage;

namespace IPT.Common.User
{
    /// <summary>
    /// A base class for generating a plugin-specific Configuration class.
    /// </summary>
    public abstract class Configuration : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SettingInt LogLevel = Logging.GetLogLevelSetting();

        private readonly List<Setting> _allSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        protected Configuration()
        {
            _allSettings = GetAllSettings();
            SubscribeToSettings();
        }

        /// <summary>
        /// Gets a list of all settings objects.
        /// </summary>
        public List<Setting> AllSettings { get => _allSettings; }

        /// <summary>
        /// Gets a list of generic combos defined in the settings.
        /// </summary>
        /// <returns>A list of combos.</returns>
        public List<GenericCombo> GetInputCombos()
        {
            var combos = new List<GenericCombo>();
            foreach (var setting in this.AllSettings)
            {
                if (setting.GetValue() is GenericCombo combo)
                {
                    combos.Add(combo);
                }
            }

            return combos;
        }

        /// <summary>
        /// Load the settings.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Logs all of the settings.
        /// </summary>
        public void Log()
        {
            Logging.Info("============================================================");
            Logging.Info($"    {Assembly.GetCallingAssembly().GetName().Name} Configuration");
            Logging.Info("============================================================");
            foreach (var entry in this.AllSettings)
            {
                Logging.Info($"{entry.Name,-30} = {entry.GetValue()}");
            }

            Logging.Info("============================================================");
        }

        /// <summary>
        /// Loads settings from INI file.
        /// </summary>
        /// <param name="filename">The filename of the INI file.  Expects path relative to the GTAV folder.</param>
        protected void LoadINI(string filename)
        {
            var ini = new InitializationFile(filename);
            if (ini.Exists())
            {
                foreach (var setting in this.AllSettings)
                {
                    try
                    {
                        setting.Load(ini);
                    }
                    catch (Exception ex)
                    {
                        Logging.Warning($"could not load setting {setting.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Saves settings to the INI file.
        /// </summary>
        /// <param name="filename">The filename of the INI file.  Expects path relative to the GTAV folder.</param>
        protected void SaveINI(string filename)
        {
            InitializationFile ini = new InitializationFile(filename);
            if (ini.Exists())
            {
                ini.Delete();
            }

            ini.Create();
            foreach (var entry in this.AllSettings)
            {
                entry.Save(ini);
            }
        }

        private List<Setting> GetAllSettings()
        {
            var settings = new List<Setting>();
            foreach (var field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.GetValue(this) is Setting setting)
                {
                    settings.Add(setting);
                }
            }

            return settings;
        }

        private void OnSettingValueChanged(Setting setting)
        {
            OnPropertyChanged(setting.Name);
        }

        private void SubscribeToSettings()
        {
            foreach (var setting in _allSettings) setting.OnValueChanged += this.OnSettingValueChanged;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
