namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a Parent used by elements to determine their positioning.
    /// </summary>
    public interface IParent : ISpatial
    {
        /// <summary>
        /// Gets the scaling factor to be applied to the container and its items.
        /// </summary>
        float Scale { get; }
    }
}
