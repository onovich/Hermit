using System;
using UnityEngine;

namespace Polyperfect.War
{
    public class AnimatorIKCallback_Forwarder:MonoBehaviour
    {
        Action<int> onIK = (a) => { };

        public void RegisterIKCallback(Action<int> act)
        {
            onIK += act;
        }

        public void UnregisterIKCallback(Action<int> act)
        {
            onIK -= act;
        }

        void OnAnimatorIK(int layerIndex)
        {
            onIK(layerIndex);
        }
    }
}