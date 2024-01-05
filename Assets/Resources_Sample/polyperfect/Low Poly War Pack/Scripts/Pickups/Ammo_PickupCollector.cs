using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Ammo_Carrier))]
    public class Ammo_PickupCollector : PickupCollector<AmmoBox_Pickup>
    {
        Ammo_Carrier ammoLibrary;
        void Awake() => ammoLibrary = GetComponent<Ammo_Carrier>();

        protected override void PickUp(AmmoBox_Pickup ammoBoxPickup) => ammoLibrary.TryAddRounds(ammoBoxPickup.AmmoType,ammoBoxPickup.Count);
        public override bool CanPickup(AmmoBox_Pickup pickup)=> ammoLibrary.SupportsAmmo(pickup.AmmoType);
        

        public override string __Usage => $"Picks up {nameof(AmmoBox_Pickup)}s and adds their contents to the attached {nameof(Ammo_Carrier)}.";
    }
}