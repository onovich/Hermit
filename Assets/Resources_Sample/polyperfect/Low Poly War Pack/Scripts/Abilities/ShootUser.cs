namespace Polyperfect.War
{
    public class ShootUser : CapabilityUser
    {
        public override string __Usage => $"Sends {(Capability?Capability.name:"NULL")} events to the actively held {nameof(Usable)}. This object is considered the User.";
        protected override bool CheckCondition() => input[Inputs.Shoot].Value;
    }
}