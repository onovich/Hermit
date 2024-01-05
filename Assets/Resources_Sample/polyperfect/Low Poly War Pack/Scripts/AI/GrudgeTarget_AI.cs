using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(AggressionTarget))]
    [RequireComponent(typeof(GrudgeHolder))]
    public class GrudgeTarget_AI : FuzzyComponent
    {
        AggressionTarget shootTarget;
        GrudgeHolder grudgeHolder;
        public override string __Usage => $"Sets the {nameof(AggressionTarget)} to the held grudge.";
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Targeting;
        protected override void Initialize()
        {
            shootTarget = GetComponent<AggressionTarget>();
            grudgeHolder = GetComponent<GrudgeHolder>();
        }

        protected override float CalculateWeight() => grudgeHolder.GetWeight();

        public override void ActiveUpdate()
        {
            shootTarget.Target = grudgeHolder.GrudgeTarget;
        }

        protected override void HandleActivate()
        {
            
        }

        protected override void HandleDeactivate()
        {
            shootTarget.Target = null;
        }
    }
}