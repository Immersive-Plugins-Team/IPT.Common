using IPT.Common.API;
using Rage;

namespace IPT.Common.User
{
    public class SettingInt : Setting
    {
        public SettingInt(string section, string name, string description, int defaultValue, int min, int max, int increment)
            : base(section, name, description)
        {
            this.Value = defaultValue;
            this.Min = min;
            this.Max = max;
            this.Increment = increment;
        }

        public int Value { get; private set; }

        public int Min { get; private set; }

        public int Max { get; private set; }

        public int Increment { get; private set; }

        public override object GetValue()
        {
            return this.Value;
        }

        public override void SetValue(object value)
        {
            this.Value = Math.Snap((int)value, this.Min, this.Max, this.Increment);
        }

        public override void Load(InitializationFile ini)
        {
            this.Value = ini.ReadInt32(this.Section, this.Name, this.Value);
            this.Value = Math.Snap(this.Value, this.Min, this.Max, this.Increment);
        }

        public override void Save(InitializationFile ini)
        {
            ini.Write(this.Section, this.Name, this.Value);
        }
    }
}
