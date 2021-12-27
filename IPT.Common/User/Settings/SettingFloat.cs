using IPT.Common.API;
using Rage;

namespace IPT.Common.User
{
    public class SettingFloat : Setting
    {
        public SettingFloat(string section, string name, string description, float defaultValue, float min, float max, float increment)
            : base(section, name, description)
        {
            this.Value = defaultValue;
            this.Min = min;
            this.Max = max;
            this.Increment = increment;
        }

        public float Value { get; private set; }

        public float Min { get; private set; }

        public float Max { get; private set; }

        public float Increment { get; private set; }

        public override object GetValue()
        {
            return this.Value;
        }

        public override void SetValue(object value)
        {
            this.Value = Math.Snap((float)value, this.Min, this.Max, this.Increment);
        }

        public override void Load(InitializationFile ini)
        {
            this.Value = System.Convert.ToSingle(ini.ReadDouble(this.Section, this.Name, this.Value));
            this.Value = Math.Snap(this.Value, this.Min, this.Max, this.Increment);
        }

        public override void Save(InitializationFile ini)
        {
            ini.Write(this.Section, this.Name, this.Value);
        }
    }
}
