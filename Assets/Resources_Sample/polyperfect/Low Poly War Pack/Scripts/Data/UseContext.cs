using JetBrains.Annotations;
using UnityEngine;

namespace Polyperfect.War
{
    [System.Serializable]
    public class UseContext
    {
        [NotNull] public UsableCapability Capability;
        [NotNull] public GameObject User;

        public UseContext([NotNull] UsableCapability capability, [NotNull] GameObject user)
        {
            Capability = capability;
            User = user;
        }
    }
}