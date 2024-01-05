using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Usable_Carrier))]
    
    public class Usable_PickupCollector : PickupCollector<Usable_Pickup>
    {
        Usable_Carrier usableLibrary;
        Ammo_Carrier ammoLibrary;
        void Awake()
        {
            usableLibrary = GetComponent<Usable_Carrier>();
            ammoLibrary = GetComponent<Ammo_Carrier>();
        }

        protected override void PickUp(Usable_Pickup pickup)
        {
            usableLibrary.AddUsable(pickup.UsablePrefab);
        }

        public override bool CanPickup(Usable_Pickup pickup)
        {
            var ret = !usableLibrary.HasUsable(pickup.UsablePrefab);
            if (ret&&pickup.UsablePrefab.TryGetComponent(out Ammo_Reservoir ammoReservoir))
                ret&=ammoLibrary && ammoLibrary.SupportsAmmo(ammoReservoir.AmmoType);

            return ret;
        }

        public override string __Usage => $"Picks up {nameof(Usable_Pickup)}s and adds their {nameof(Usable)} to the attached {nameof(Usable_Carrier)}.";
    }
}