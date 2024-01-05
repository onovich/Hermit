using UnityEngine;

namespace Polyperfect.War
{
    
    [RequireComponent(typeof(Energy_Reservoir))]
    public class Energy_UseEffector : UseEffector
    {
        public override string __Usage => $"Makes the attached {nameof(usable)} require and consume energy from the attached {nameof(Energy_Reservoir)}.;";
        [SerializeField] float UsedEnergy = .5f;
        Energy_Reservoir energy;
        protected override void Initialize() => energy = GetComponent<Energy_Reservoir>();
        public static readonly Reason NotEnoughEnergy = new Reason();
        public override bool CheckCondition(UseContext context, out Reason reason)
        {
            var ret = energy.CanFullyExtract(UsedEnergy);
            reason = ret ? Reason.None : NotEnoughEnergy;
            return ret;
        }

        public override void OnUse(UseContext context) => energy.ExtractPossible(UsedEnergy);
    }
}