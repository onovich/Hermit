using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public abstract class PickupCollector<T> : PickupCollector where T:Pickup
    {
        protected void OnTriggerEnter(Collider other)
        {
            var pickup = other.GetComponent<T>();
            if (!pickup || !pickup.enabled || !CanPickup(pickup))
                return;
            
            pickup.GetPickUp(this);
            PickUp(pickup);
            OnPickUp.Invoke();
        }

        public abstract bool CanPickup(T pickup);
        protected abstract void PickUp(T pickup);
    }

    public abstract class PickupCollector : PolyMono
    {
        public UnityEvent OnPickUp;
    }
}