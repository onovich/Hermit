using System;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Rider_Carrier))]
    public class Vehicle_UsableHolder : PolyMono,IUsableHolder
    {
        public override string __Usage => "References the weapons attached to the vehicle. Causes the UseContext User to reference the rider, instead of the vehicle itself.";
        Usable[] usables;// attachedUsable;
        Rider_Carrier riderLibrary;
        [SerializeField] UnityEvent<Usable> onUse;
        [SerializeField] UnityEvent<UseFailedContext> onFailedUse;
        void Start()
        {
            usables = GetComponentsInChildren<Usable>().Where(u => u.gameObject != gameObject).ToArray();
            riderLibrary = GetComponent<Rider_Carrier>();
        }

        public void Use(UsableCapability capability)
        {
            foreach (var attachedUsable in usables)
            {
                if (!attachedUsable.TryUse(new UseContext(capability,GetUser()),out var failure))
                    onFailedUse.Invoke(failure);
            }
        }

        GameObject GetUser()
        {
            var rider = riderLibrary.Riders.FirstOrDefault();
            return rider ? rider.gameObject : null;
        }
        public void RegisterUseCallback(UnityAction<Usable> act) => onUse.AddListener(act);
        public void UnregisterUseCallback(UnityAction<Usable> act) => onUse.RemoveListener(act);
        public void RegisterUseFailureCallback(UnityAction<UseFailedContext> act) => onFailedUse.AddListener(act);
        public void UnregisterUseFailureCallback(UnityAction<UseFailedContext> act) => onFailedUse.RemoveListener(act);
        public bool CheckConditions(UseContext context, Func<IUseCondition, bool> conditionFilter)
        {
            return usables.Any(u => u.CheckConditions(context, conditionFilter));
        }
    }
}