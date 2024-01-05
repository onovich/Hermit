using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(DynamicUsableHolder))]
    
    public class AmmoReservoir_Proxy : ChangeableBase<ActiveAmmoInfo>
    {
        public override string __Usage => $"Updates to reference the ammo in the {nameof(Ammo_Reservoir)} (if any) held by the {nameof(DynamicUsableHolder)}.";
        DynamicUsableHolder usableHolder;
        Ammo_Reservoir ammoTarget;
        public ActiveAmmoInfo Ammo
        {
            get => internalValue;
            set => TryChangeTo(value);
        }

        protected  void Awake()
        {
            usableHolder = GetComponent<DynamicUsableHolder>();
            usableHolder.RegisterChangeCallback(HandleUsableChange);
        }

        void HandleUsableChange(ChangeEvent<Usable> evt)
        {
            if (ammoTarget)
                ammoTarget.UnregisterChangeCallback(HandleAmmoChange);

            ammoTarget = null;
            if (evt.Next)
            {
                ammoTarget = evt.Next.GetComponent<Ammo_Reservoir>();
                if (ammoTarget)
                {
                    ammoTarget.RegisterChangeCallback(HandleAmmoChange);
                }
            }
            UpdateAmmo();
        }

        void HandleAmmoChange(ChangeEvent<int> evt)
        {
            UpdateAmmo();
        }

        void UpdateAmmo()
        {
            Ammo = ammoTarget ? 
                new ActiveAmmoInfo(ammoTarget.AmmoType, ammoTarget.AbsoluteAmount, ammoTarget.MaxAmount) : 
                new ActiveAmmoInfo(null, 0, 0);
        }
    }
}