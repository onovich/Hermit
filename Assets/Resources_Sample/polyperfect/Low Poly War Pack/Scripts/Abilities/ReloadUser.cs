namespace Polyperfect.War
{
    public class ReloadUser : CapabilityUser
    {
        public override string __Usage => $"Sends {(Capability?Capability.name:"NULL")} events to the actively held {nameof(Usable)}.";
        protected override bool CheckCondition() => input[Inputs.Reload].Value;
    }
}