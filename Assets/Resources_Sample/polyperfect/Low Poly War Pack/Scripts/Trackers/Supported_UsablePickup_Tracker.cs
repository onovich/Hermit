namespace Polyperfect.War
{
    public class Supported_UsablePickup_Tracker : Tracker<Usable_Pickup>
    {
        Usable_PickupCollector collector;

        void Awake() => collector = GetComponent<Usable_PickupCollector>();

        public override string __Usage => $"Tracks {nameof(Usable_Pickup)}s in range.";

        protected override bool ShouldTrack(Usable_Pickup targetable) => collector.CanPickup(targetable);
    }
}