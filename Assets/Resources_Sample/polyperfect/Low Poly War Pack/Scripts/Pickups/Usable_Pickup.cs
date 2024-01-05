namespace Polyperfect.War
{
    public class Usable_Pickup : Pickup
    {
        public override string __Usage => $"Grants a {nameof(Usable)} when collected. Automatically tracked by {nameof(Supported_UsablePickup_Tracker)}.";
        public Usable UsablePrefab;

        protected override void Initialize() { }
    }
}