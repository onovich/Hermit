using System;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public interface IUsableHolder : IUser, IUserEvents,IUseChecker
    {
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Ammo_Carrier))]
    public abstract class DynamicUsableHolder : ChangeableBase<Usable>,IUsableHolder
    {
        [SerializeField] protected UnityEvent<Usable> OnUse;
        [SerializeField] protected UnityEvent<UseFailedContext> OnUseFailed;
        public Usable Usable
        {
            get => internalValue;
            set => TryChangeTo(value);
        }

        protected  void Awake()
        {
            RegisterChangeCallback(ActivateAndDeactivate);
        }
        void ActivateAndDeactivate(ChangeEvent<Usable> evt)
        {
            if (evt.Previous)
                evt.Previous.gameObject.SetActive(false);
            if(evt.Next)
                evt.Next.gameObject.SetActive(true);
        }

        public void AimAt(Vector3 target)
        {
            if (Usable)
                Usable.transform.LookAt(target);
        }
        public void SetUsableLocalRotation(Quaternion rot)
        {
            if (Usable)
                Usable.transform.localRotation = rot;
        }
        
        public void ClearUsableRotation() => SetUsableLocalRotation(Quaternion.identity);
        
        public void Use(UsableCapability capability)
        {
            if (Usable)
            {
                if (!Usable.TryUse(new UseContext(capability, gameObject), out var failed))
                {
                    OnUseFailed.Invoke(failed);
                }
            }
        }
    
        public abstract Usable CreateUsable(GameObject usablePrefab);
        public void RegisterUseCallback(UnityAction<Usable> act) => OnUse.AddListener(act);
        public void UnregisterUseCallback(UnityAction<Usable> act) => OnUse.RemoveListener(act);
        public void RegisterUseFailureCallback(UnityAction<UseFailedContext> act) => OnUseFailed.AddListener(act);
        public void UnregisterUseFailureCallback(UnityAction<UseFailedContext> act) => OnUseFailed.RemoveListener(act);
        public bool CheckConditions(UseContext context, Func<IUseCondition, bool> conditionFilter)
        {
            if (!Usable)
                return false;
            return Usable.CheckConditions(context, conditionFilter);
        }
    }
}