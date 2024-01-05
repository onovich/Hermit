using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class MatchRotation : PolyMono
    {
        public override string __Usage => "Copies the rotation of the target object, optionally about some specified axis.";
        public Transform Target;
        public bool UseUp = true;
        public Vector3 UpAxis = Vector3.up;

        void Update()
        {
            transform.rotation = UseUp ? Quaternion.LookRotation(Vector3.ProjectOnPlane(Target.forward, UpAxis).normalized, UpAxis) : Target.rotation;
        }
    }
}