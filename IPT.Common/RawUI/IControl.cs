namespace IPT.Common.RawUI
{
    /// <summary>
    /// Represents an object that can be used for controlling things like a button.
    /// </summary>
    public interface IControl
    {
        /// <summary>
        /// Executed when the user clicks the control.
        /// </summary>
        void Click();
    }
}
