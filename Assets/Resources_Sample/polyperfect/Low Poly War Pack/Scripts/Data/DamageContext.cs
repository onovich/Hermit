using JetBrains.Annotations;
using UnityEngine;

namespace Polyperfect.War
{
    [System.Serializable]
    public struct DamageContext
    {
        public float DamageAmount { get; set; }
        public Vector3 ImpactVector { get; set; }
        public Vector3 ImpactPosition { get; set; }
        [CanBeNull] public UseContext UseContext { get; set; }

        public DamageContext(float damageAmount, Vector3 impactVector, [CanBeNull] UseContext useContext, Vector3 impactPosition)
        {
            DamageAmount = damageAmount;
            ImpactVector = impactVector;
            UseContext = useContext;
            ImpactPosition = impactPosition;
        }
    }
}