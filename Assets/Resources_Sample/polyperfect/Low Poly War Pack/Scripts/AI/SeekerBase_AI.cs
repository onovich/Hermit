using System.Linq;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(InputCollector))]
    public abstract class SeekerBase_AI<TrackerType,TrackedType> : FuzzyComponent where TrackerType:Tracker<TrackedType> where TrackedType:class,ITargetable
    {
        [Range(0,1f)]
        public float SpeedMultiplier = .5f;
        public float SearchRadius = 20f;
        public bool Run = true;
        InputCollector input;
        TiltTowardsPosition_Input tilter;
        protected TrackerType tracker;
        TrackedType target;
        Vector3 previousPosition;
        protected override void Initialize()
        {
            tilter = gameObject.AddAndDisableComponent<TiltTowardsPosition_Input>();
            tilter.StopDistance = 0f;
            tracker = gameObject.AddComponent<TrackerType>();
            input = GetComponent<InputCollector>();
            tracker.Radius = SearchRadius;
        }

        public override void ActiveUpdate()
        {
            
            if ((target == null)||!tracker.Tracked.Contains(target))
            {
                var pos = transform.position;
                target = tracker.Tracked.MinBy(t=>Vector3.Distance(t.Position,pos));
                
            }
            if (target!=null && (target.Position - previousPosition).sqrMagnitude > .1f)
            {
                tilter.SetTarget(target.Position, SpeedMultiplier);
                previousPosition = target.Position;
            }
        }

        protected override float CalculateWeight() => tracker.Tracked.Any()?1f:0f;

        bool wasRunning = false;
        protected override void HandleActivate()
        {
            tilter.enabled = true;
            wasRunning = input[Inputs.Run].Value;
            input[Inputs.Run].Set(Run);
        }

        protected override void HandleDeactivate()
        {
            tilter.enabled = false;
            input[Inputs.Run].Set(wasRunning);
        }
    }
}