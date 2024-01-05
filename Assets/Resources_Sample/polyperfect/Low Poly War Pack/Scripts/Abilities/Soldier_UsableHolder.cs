using System;
using System.Linq;
using UnityEngine;

namespace Polyperfect.War
{
    public class Soldier_UsableHolder : DynamicUsableHolder
    {
        public override string __Usage => $"Holds {nameof(Usable)}s, typically weapons. Change event is upon the held {nameof(Usable)} being changed. Will extract ammo from weapons on swapping them off, and put it in the {nameof(Ammo_Carrier)} if available.";
        
        public Transform RightHand;
        void Reset()
        {
            RightHand = transform.AllChildren().FirstOrDefault(t => t.name.Contains("Hand") && t.name.Contains("_R"));
        }
        

         void Start()
        {
            if (!RightHand)
            {
                Debug.LogError($"{nameof(Soldier_UsableHolder)} on {name} requires hands to attach weapons to.");
                enabled = false;
                return;
            }
            RegisterChangeCallback(HandleChange);
        }

        void HandleChange(ChangeEvent<Usable> arg0)
        {
            if (arg0.Previous && arg0.Previous.TryGetComponent(out Ammo_Reservoir reservoir))
            {
                if (gameObject.TryGetComponent(out Ammo_Carrier carrier))
                {
                    carrier.TryAddRounds(reservoir.AmmoType, reservoir.ExtractAll().Amount);
                }
            }
        }

        public override Usable CreateUsable(GameObject usablePrefab)
        {
            var usableObj = Instantiate(usablePrefab, RightHand.transform);
            var ret = usableObj.GetComponent<Usable>();
            
            if (ret)
                return ret;
            throw new Exception($"Created weapons are expected to have a {nameof(War.Usable)} attached. {usablePrefab.name} did not.");
        }
    }
}