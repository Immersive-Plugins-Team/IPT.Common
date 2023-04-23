namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents a Parent used by drawables to determine their positioning.
    /// </summary>
    public interface IParent : ISpatial
    {
        /// <summary>
        /// Gets the scaling factor to be applied to the parent and its children.
        /// </summary>
        float Scale { get; }
    }
}
