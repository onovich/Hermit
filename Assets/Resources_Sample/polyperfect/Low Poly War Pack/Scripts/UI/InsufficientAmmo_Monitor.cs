namespace Polyperfect.War
{
    public class InsufficientAmmo_Monitor : FailureConditionMonitor
    {
        public override string __Usage => $"Triggers an event when there is not enough ammo to shoot.";
        protected override bool ConditionMet(UseFailedContext context) => context.Reason == Ammo_UseEffector.NotEnoughAmmo;
    }
}