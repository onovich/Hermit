using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class ConstrainedAxisTurning : PolyMono
    {
        public override string __Usage => $"Rotates the object around the target axis towards {nameof(TargetDirection)} while respecting the prescribed limits.";

        [Tooltip("Maximum rotation speed in Radians per Second")] [Range(.1f, 80f)]
        public float TurnSpeed = 20f;

        public Vector3 TargetDirection = Vector3.forward;
        public Vector3 LocalAxis = Vector3.up;
        public float MaxAngle = 30f;
        public float MinAngle = -30f;
        Quaternion restRotation;

        void Start()
        {
            restRotation = transform.localRotation;
        }
        
        void Update()
        {
            var trans = transform;
            var axis = trans.TransformDirection(LocalAxis);
            var curForward = Vector3.ProjectOnPlane(trans.forward, axis);
            var targetForward = Vector3.ProjectOnPlane(TargetDirection, axis);
            var parentRot = Quaternion.identity;
            var parent = transform.parent;
            
            if (parent)
                parentRot = parent.rotation;
            var restForward = parentRot * restRotation * Vector3.forward;
            var targetAngle = Mathf.Clamp(Vector3.SignedAngle(restForward, targetForward, axis),MinAngle,MaxAngle);
            targetForward = Vector3.RotateTowards(restForward, targetForward, Mathf.Abs(targetAngle * Mathf.Deg2Rad), 0f);
            var newForward = Vector3.RotateTowards(curForward, targetForward, TurnSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newForward, trans.up);
        }
    }
}