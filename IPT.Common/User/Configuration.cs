using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IPT.Common.API;
using IPT.Common.User.Inputs;
using IPT.Common.User.Settings;
using Rage;

namespace IPT.Common.User
{
    /// <summary>
    /// A base class for generating a plugin-specific Configuration class.
    /// </summary>
    public abstract class Configuration
    {
        #pragma warning disable S1104, SA1401, CS1591, SA1600
        public SettingInt LogLevel = Logging.GetLogLevelSetting();
        #pragma warning restore S1104, SA1401, CS1591, SA1600

        private readonly List<Setting> allSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        protected Configuration()
        {
            this.allSettings = this.GetAllSettings();
        }

        /// <summary>
        /// Gets a list of all settings objects.
        /// </summary>
        public List<Setting> AllSettings
        {
            get
            {
                return this.allSettings;
            }
        }

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
            foreach (var field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(
                x => x.FieldType.BaseType == typeof(Setting)))
            {
                try
                {
                    settings.Add((Setting)field.GetValue(this));
                }
                catch (Exception ex)
                {
                    Logging.Error($"could not retrieve setting: {field.Name}", ex);
                }
            }

            return settings;
        }
    }
}
