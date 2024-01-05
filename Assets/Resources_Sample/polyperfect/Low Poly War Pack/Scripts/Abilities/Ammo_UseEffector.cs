using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Ammo_Reservoir))]
    public class Ammo_UseEffector : UseEffector
    {
        public override string __Usage => $"Makes the attached {nameof(usable)} require and consume ammo from the attached {nameof(Ammo_Reservoir)}.";
        [SerializeField] int UsedAmmo = 1;
        Ammo_Reservoir ammo;
        public static readonly Reason NotEnoughAmmo = new Reason();
        protected override void Initialize() => ammo = GetComponent<Ammo_Reservoir>();

        public override bool CheckCondition(UseContext context, out Reason reason)
        {
            var ret = ammo.CanFullyExtract(UsedAmmo);
            reason = ret?Reason.None:NotEnoughAmmo;
            return ret;
        }

        public override void OnUse(UseContext context) => ammo.ExtractPossible(UsedAmmo);
    }
}