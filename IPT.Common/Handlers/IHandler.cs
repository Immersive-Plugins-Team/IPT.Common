namespace IPT.Common.Handlers
{
    /// <summary>
    /// An interface for Handler classes.
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Starts the handler.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the handler.
        /// </summary>
        void Stop();
    }
}
