using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class  RotateToUp : PolyMono
    {
        public override string __Usage => "Makes the target align itself.";
        public Vector3 LocalDirection = Vector3.up;
        public Vector3 AlignDirection = Vector3.up;

        void Update()
        {
            transform.rotation = Quaternion.FromToRotation(transform.TransformDirection(LocalDirection), AlignDirection)*transform.rotation;
        }
    }
}