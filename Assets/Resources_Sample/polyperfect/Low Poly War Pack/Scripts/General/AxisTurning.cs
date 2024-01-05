using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class AxisTurning : PolyMono
    {
        public override string __Usage => $"Rotates the object around the target axis towards {nameof(TargetDirection)}.";

        [Tooltip("Maximum rotation speed in Radians per Second")] [Range(.1f, 80f)]
        public float TurnSpeed = 20f;

        public Vector3 TargetDirection = Vector3.forward;
        public Vector3 LocalAxis = Vector3.up;

        void Update()
        {
            var trans = transform;
            var axis = trans.TransformDirection(LocalAxis);
            var curForward = Vector3.ProjectOnPlane(trans.forward, axis);
            var targetForward = Vector3.ProjectOnPlane(TargetDirection, axis);
            var newForward = Vector3.RotateTowards(curForward, targetForward, TurnSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newForward, trans.up);
        }
    }
}