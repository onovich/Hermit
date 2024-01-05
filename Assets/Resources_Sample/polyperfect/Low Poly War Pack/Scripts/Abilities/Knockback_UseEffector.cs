using UnityEngine;

namespace Polyperfect.War
{
    public class Knockback_UseEffector : UseEffect
    {
        public override string __Usage => "Causes a force upon being used.";
        public Vector3 LocalForceVector = Vector3.up*1000f;
        Rigidbody rb;
        [SerializeField] Transform ForceTransform;
        Transform usedTransform => ForceTransform ? ForceTransform : transform;

        protected override void Initialize()
        {
            rb = GetComponentInParent<Rigidbody>();
            if (!rb)
            {
                Debug.LogError($"{nameof(Knockback_UseEffector)} requires it or a parent to have a Rigidbody attached.");
                enabled = false;
            }
        }

        public override void OnUse(UseContext context) => rb.AddForceAtPosition(usedTransform.TransformDirection(LocalForceVector),usedTransform.position,ForceMode.Impulse);
    }
}