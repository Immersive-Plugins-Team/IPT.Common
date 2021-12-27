using Rage;

namespace IPT.Common.User
{
    public abstract class Setting
    {
        protected Setting(string section, string name, string description)
        {
            this.Section = section;
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; private set; }

        public string Section { get; private set; }

        public string Description { get; private set; }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public abstract void Load(InitializationFile ini);

        public abstract void Save(InitializationFile ini);
    }
}
