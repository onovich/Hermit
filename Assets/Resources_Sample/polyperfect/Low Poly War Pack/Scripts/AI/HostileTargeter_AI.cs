using System;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(AggressionTarget))]
    [RequireComponent(typeof(FactionReference))]
    public class HostileTargeter_AI : FuzzyComponent
    {
        public override string __Usage => $"Sets {nameof(AggressionTarget)} to hostiles in range and moves towards them when active.";
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Targeting;
        [SerializeField] bool DoMovement = true;
        
        TiltTowardsPosition_Input tilter;
        public float HostileCheckRadius = 30f;
        public float TiltStrength = 1f;
        public float StopDistance = 10f;
        Hostile_Tracker tracker;
        public enum TargetingMode
        {
            Weakest,
            Strongest,
            Nearest
        }

        public TargetingMode Targeting;
        AggressionTarget toShoot;

        void Reset()
        {
            WeightMultiplier = .8f;
        }

        protected override void Initialize()
        {
            if (DoMovement)
            {
                tilter = gameObject.AddAndDisableComponent<TiltTowardsPosition_Input>();
                tilter.StopDistance = StopDistance;
            }

            tracker = gameObject.AddComponent<Hostile_Tracker>();
            tracker.Radius = HostileCheckRadius;
            toShoot = GetComponent<AggressionTarget>();
        }

        protected override float CalculateWeight()
        {
            return tracker.Tracked.Any() ? 1f : 0f;
        }

        public override void ActiveUpdate()
        {
            var position = transform.position;
            Shootable_Target target;
            switch (Targeting)
            {
                case TargetingMode.Nearest:
                    target = tracker.Tracked.MinBy(t => Vector3.Distance(position, t.Position));
                    break;
                case TargetingMode.Strongest:
                    target = tracker.Tracked.MaxBy(t => t.AttachedHealth.Health);
                    break;
                case TargetingMode.Weakest:
                    target = tracker.Tracked.MinBy(t => t.AttachedHealth.Health);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            toShoot.Target = target;
            if (tilter&&target)
                tilter.SetTarget(target.Position,TiltStrength);
        }

        protected override void HandleActivate() { }

        protected override void HandleDeactivate()
        {
            toShoot.Target = null;
        }
    }
}