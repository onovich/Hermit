using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Health_Reservoir))]
    [RequireComponent(typeof(Medkit_PickupCollector))]
    public class Medkit_Tracker : Tracker<Medkit_Pickup>
    {
        Medkit_PickupCollector collector;
        public override string __Usage => $"Tracks {nameof(Medkit_Pickup)}s in range.";
        protected override bool ShouldTrack(Medkit_Pickup targetable) => collector.CanPickup(targetable);

        void Awake() => collector = GetComponent<Medkit_PickupCollector>();
    }
}