using System.Collections.Generic;
using IPT.Common.API;

namespace IPT.Common
{
    public abstract class CommonPlugin : LSPD_First_Response.Mod.API.Plugin
    {
        protected abstract string PluginName { get; }
        protected abstract List<Dependency> Dependencies { get; }

        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += this.Functions_OnOnDutyStateChanged;
        }

        public override void Finally()
        {
            Stop();
        }

        protected abstract void Start();
        protected abstract void Stop();

        private void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
                if (DependencyManager.Check(PluginName, Dependencies)) Start();
        }
    }
}
