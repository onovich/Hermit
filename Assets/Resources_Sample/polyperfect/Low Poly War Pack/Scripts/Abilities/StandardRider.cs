using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace Polyperfect.War
{
    public class StandardRider : RiderBase
    {
        public override string __Usage => $"Attaches to {nameof(RideEnter_UseEffector)} using a {nameof(ParentConstraint)}.";
        [SerializeField] MonoBehaviour[] DisableOnRide;
        readonly List<MonoBehaviour> disabled = new List<MonoBehaviour>();
        ParentConstraint parentConstraint;
        protected new void Awake()
        {
            base.Awake();
            parentConstraint = gameObject.AddComponent<ParentConstraint>();
            input = GetComponent<InputCollector>();
        }

        protected override void HandleEnterRideable(RideEnterContext context)
        {
            foreach (var item in DisableOnRide)
            {
                if (item.enabled)
                {
                    disabled.Add(item);
                    item.enabled = false;
                }
            }
            parentConstraint.AddSource(new ConstraintSource(){sourceTransform = context.Rideable.GetSlot(),weight = 1f});
            parentConstraint.constraintActive = true;
        }

        protected override void HandleExitRideable(RideExitContext context)
        {
            if (!parentConstraint)
                return;
            foreach (var item in disabled.Where(s=>s)) 
                item.enabled = true;
            disabled.Clear();
            parentConstraint.constraintActive = false;
            parentConstraint.RemoveSource(0);

            var transform1 = transform;
            transform1.position = context.SpawnPosition;
            transform1.rotation = context.SpawnRotation;
        }
    }
}