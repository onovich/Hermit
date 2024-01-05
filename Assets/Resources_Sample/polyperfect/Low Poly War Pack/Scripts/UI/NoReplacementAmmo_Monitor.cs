namespace Polyperfect.War
{
    public class NoReplacementAmmo_Monitor : FailureConditionMonitor
    {
        public override string __Usage => "Triggers an event when no ammo is available to reload with.";
        protected override bool ConditionMet(UseFailedContext context) => context.Reason == Reload_UseEffector.NoAvailableAmmo;
    }
}