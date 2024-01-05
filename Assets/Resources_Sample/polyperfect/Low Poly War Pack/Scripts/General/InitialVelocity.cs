using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Rigidbody))]
    public class InitialVelocity : PolyMono
    {
        public override string __Usage => $"Applies the velocity to the attached {nameof(Rigidbody)} on {nameof(Start)}.";
        public Vector3 LocalVelocity = Vector3.forward * 10f;
        void Start()
        {
            var body = GetComponent<Rigidbody>();
            body.AddForce(transform.rotation*LocalVelocity,ForceMode.VelocityChange);
        }
    }
}