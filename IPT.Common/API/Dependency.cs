namespace IPT.Common.API
{
    /// <summary>
    /// Represents a single plugin dependency with a name and minimum required version.
    /// </summary>
    public class Dependency
    {
        public string Name { get; }
        public string Version { get; }

        public Dependency(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}
