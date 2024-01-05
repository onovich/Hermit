using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(DynamicUsableHolder))]
    [RequireComponent(typeof(AggressionTarget))]
    public class ShootTargetSeek_AI : FuzzyComponent
    {
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Movement;
        public override string __Usage => $"Makes the entity move towards the target in the attached {nameof(AggressionTarget)}.";
        public float FollowDistance = 5f;
        [SerializeField] UsableCapability ShootCapability;
        TiltTowardsPosition_Input tilter;
        AggressionTarget target;
        DynamicUsableHolder holder;
        protected override void Initialize()
        {
            tilter = gameObject.AddAndDisableComponent<TiltTowardsPosition_Input>();
            tilter.StopDistance = FollowDistance;
            target = GetComponent<AggressionTarget>();
            holder = GetComponent<DynamicUsableHolder>();
        }

        protected override float CalculateWeight()
        {
            var canAttack = target.CanShootAndVisible;
            return canAttack ? 1f : 0f;
        }

        public override void ActiveUpdate()
        {
            tilter.SetTarget(target.Target?.Position ?? transform.position);
        }


        protected override void HandleActivate() => tilter.enabled = true;

        protected override void HandleDeactivate() => tilter.enabled = false;
    }
}