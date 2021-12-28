namespace IPT.Common
{
    public abstract class CommonPlugin : LSPD_First_Response.Mod.API.Plugin
    {
        public override void Finally()
        {
            this.Stop();
        }

        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += this.Functions_OnOnDutyStateChanged;
        }

        private void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                this.Start();
            }
        }

        protected abstract void Start();

        protected abstract void Stop();
    }
}
