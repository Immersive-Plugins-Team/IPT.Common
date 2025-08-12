using LSPD_First_Response.Mod.API;
using System;

namespace IPT.Common
{
    /// <summary>
    /// A robust base class for LSPDFR plugins that correctly handles the RPH/LSPDFR lifecycle.
    /// </summary>
    public abstract class PluginBase : Plugin
    {
        /// <summary>
        /// Called once by RPH when the plugin is first loaded. This is the only time this method is called.
        /// Use it to create your composition root and other long-lived objects.
        /// </summary>
        public abstract void OnPluginLoaded();

        /// <summary>
        /// Called once by RPH when the plugin is about to be fully unloaded from the AppDomain.
        /// Use it to dispose of all resources.
        /// </summary>
        public abstract void OnPluginUnloaded();

        /// <summary>
        /// Called every time the player goes on duty.
        /// Use it to start your plugin's fibers and services.
        /// </summary>
        public abstract void OnDuty();

        /// <summary>
        /// Called every time the player goes off duty.
        /// Use it to stop your plugin's fibers and services.
        /// </summary>
        public abstract void OffDuty();

        /// <summary>
        /// The RPH entry point. Do not override this in your derived class.
        /// </summary>
        public override sealed void Initialize()
        {
            // The base class handles all the complex wiring.
            Functions.OnOnDutyStateChanged += OnDutyStateChangedHandler;
            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
            OnPluginLoaded();
        }

        /// <summary>
        /// Fucking pointless.
        /// </summary>
        public override sealed void Finally() { }

        private void OnDutyStateChangedHandler(bool onDuty)
        {
            if (onDuty) OnDuty();
            else OffDuty();
        }

        private void OnDomainUnload(object sender, EventArgs e)
        {
            OnPluginUnloaded();
        }
    }
}
