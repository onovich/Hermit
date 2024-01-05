using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Rigidbody))]
    public class ParachuteMovement : PolyMono
    {
        public override string __Usage => "Simple Parachute-like movement.";
        public Transform Wing;
        public float NormalDrag = 1f;
        public float TangentDrag = .1f;
        public float GroundDistanceCheck = 5f;
        public UnityEvent OnGroundInRange;
        Rigidbody body;

        void Start()
        {
            body = GetComponent<Rigidbody>();
            OnGroundInRange.AddListener(() => enabled = false);
        }

        void FixedUpdate()
        {
            var wingPosition = Wing.position;
            var vel = body.GetPointVelocity(wingPosition);
            var normal = Wing.up;
            var dot = -Vector3.Dot(vel, normal);

            var normalForce = normal * (dot * NormalDrag);
            var tangentForce = -Vector3.ProjectOnPlane(vel, normal) * TangentDrag;

            var force = normalForce + tangentForce;

            body.AddForceAtPosition(force, wingPosition, ForceMode.Force);

            if (RaycastHelpers.IsNonDynamicColliderInRange(new Ray(body.position, Physics.gravity.normalized), GroundDistanceCheck))
                OnGroundInRange.Invoke();

        }
    }
}