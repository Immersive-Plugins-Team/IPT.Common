namespace IPT.Common
{
    /// <summary>
    /// An abstract generic plugin class.
    /// </summary>
    public abstract class CommonPlugin : LSPD_First_Response.Mod.API.Plugin
    {
        /// <summary>
        /// Called when the player goes off duty.
        /// </summary>
        public override void Finally()
        {
            this.Stop();
        }

        /// <summary>
        /// Called when the plugin is created.
        /// </summary>
        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += this.Functions_OnOnDutyStateChanged;
        }

        /// <summary>
        /// Starts the plugin.
        /// </summary>
        protected abstract void Start();

        /// <summary>
        /// Stops the plugin.
        /// </summary>
        protected abstract void Stop();

        private void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                this.Start();
            }
        }
    }
}
