using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class CameraPriority_UseEffect:UseEffect
    {
        public override string __Usage => "Changes Camera Priority on being used";
        [HighlightNull] [SerializeField] VirtualCamera Camera;
        [SerializeField] int TargetPriority = 20;
        public override void OnUse(UseContext context) => Camera.priority = TargetPriority;
    }
}