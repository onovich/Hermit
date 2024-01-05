using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Health_Reservoir))]
    public class Medkit_PickupCollector : PickupCollector<Medkit_Pickup>
    {
        Health_Reservoir health;
        void Awake() => health = GetComponent<Health_Reservoir>();

        protected override void PickUp(Medkit_Pickup pickup) => health.UseMedkit(pickup);
        public override bool CanPickup(Medkit_Pickup pickup) => health.FractionalAmount < 1f;

        public override string __Usage => $"Picks up and uses {nameof(Medkit_Pickup)} on the attached {nameof(Health_Reservoir)}";
    }
}