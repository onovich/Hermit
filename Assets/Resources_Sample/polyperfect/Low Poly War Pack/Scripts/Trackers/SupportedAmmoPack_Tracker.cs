using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Ammo_PickupCollector))]
    public class SupportedAmmoPack_Tracker : Tracker<AmmoBox_Pickup>
    {
        public override string __Usage => $"Tracks {nameof(AmmoBox_Pickup)}s supported by the {nameof(Ammo_PickupCollector)} in range.";
        Ammo_PickupCollector collector;

        void Awake() => collector = GetComponent<Ammo_PickupCollector>();
        protected override bool ShouldTrack(AmmoBox_Pickup targetable) => collector.CanPickup(targetable);
    }
}