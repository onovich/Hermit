using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(GunAimer))]
    [RequireComponent(typeof(AggressionTarget))]
    [RequireComponent(typeof(InputCollector))]
    public class TargetShooter_AI : FuzzyComponent,IAimEffector
    {
        public override string __Usage => $"Presses the {nameof(Inputs.Aim)} and {nameof(Inputs.Shoot)} inputs when there is an active target stored in {nameof(AggressionTarget)}.";
        GunAimer aiming;
        InputCollector inputs;
        AggressionTarget shootTarget;

        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Gunning;
        protected override void Initialize()
        {
            shootTarget = GetComponent<AggressionTarget>();
            inputs = GetComponent<InputCollector>();
            aiming = GetComponent<GunAimer>();
        }

        protected override float CalculateWeight()
        {
            
            return shootTarget.CanShootAndVisible ? 1f : 0f;
        }

        public override void ActiveUpdate()
        {
            var doShoot = shootTarget.Target!=null;
            inputs[Inputs.Shoot].Set(doShoot);
            inputs[Inputs.Aim].Set(doShoot);
        }

        protected override void HandleActivate()
        {
            aiming.SetAimingFunction(this);
        }

        protected override void HandleDeactivate()
        {
            inputs[Inputs.Shoot].Set(false);
            inputs[Inputs.Aim].Set(false);
        }

        public Vector3 GetAimPosition()
        {
            var targetable = shootTarget.Target;
            return targetable?.Position ?? transform.TransformPoint(Vector3.forward * 50f);
        }
    }
}