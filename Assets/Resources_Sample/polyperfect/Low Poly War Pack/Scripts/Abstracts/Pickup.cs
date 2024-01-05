using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public abstract class Pickup : TargetableBase
    {
        [SerializeField] bool AutoDestroy = true; 
        [SerializeField] protected UnityEvent<PickupCollector> OnPickedUp;

        protected new void Awake()
        {
            base.Awake();
            if (AutoDestroy)
                OnPickedUp.AddListener((p)=>Destroy(gameObject));
        }
        
        public void GetPickUp(PickupCollector pickup) => OnPickedUp.Invoke(pickup);
    }
}