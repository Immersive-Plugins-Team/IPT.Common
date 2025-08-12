using System;
using System.Collections.Generic;
using System.Reflection;
using IPT.Common.API;
using IPT.Common.User.Inputs;
using IPT.Common.User.Settings;
using Rage;

namespace IPT.Common.User
{
    public abstract class Configuration : IDisposable
    {
        public delegate void SettingChangedEventHandler(Setting setting);
        public event SettingChangedEventHandler SettingChanged;
        public SettingInt LogLevel = Logging.GetLogLevelSetting();

        private readonly List<Setting> _allSettings;

        protected Configuration()
        {
            _allSettings = GetAllSettings();
            foreach (var setting in _allSettings) setting.OnValueChanged += OnSettingValueChanged;
        }

        public List<Setting> AllSettings { get => _allSettings; }

        public void Dispose()
        {
            foreach (var setting in _allSettings) setting.OnValueChanged -= OnSettingValueChanged;
        }

        public List<GenericCombo> GetInputCombos()
        {
            var combos = new List<GenericCombo>();
            foreach (var setting in this.AllSettings)
            {
                if (setting.GetValue() is GenericCombo combo) combos.Add(combo);
            }

            return combos;
        }

        public abstract void Load();

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

        protected void SaveINI(string filename)
        {
            InitializationFile ini = new InitializationFile(filename);
            if (ini.Exists()) ini.Delete();
            ini.Create();
            foreach (var entry in this.AllSettings) entry.Save(ini);
        }

        private List<Setting> GetAllSettings()
        {
            var settings = new List<Setting>();
            foreach (var field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.GetValue(this) is Setting setting) settings.Add(setting);
            }

            return settings;
        }

        private void OnSettingValueChanged(Setting setting) => SettingChanged?.Invoke(setting);
    }
}
