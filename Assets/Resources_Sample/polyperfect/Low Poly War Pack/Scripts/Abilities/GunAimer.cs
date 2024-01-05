using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public abstract class GunAimer : PolyMono
    {
        [LowPriority][SerializeField] protected UnityEvent OnBeginAiming = default;

        [LowPriority] [SerializeField] protected UnityEvent OnEndAiming = default;

        protected IAimEffector activeAimer { get; private set; }

        public void SetAimingFunction(IAimEffector aimer)
        {
            activeAimer = aimer;
        }

        public Vector3 TargetAimPosition => activeAimer?.GetAimPosition() ?? Vector3.zero;
        //public abstract Vector3 CurrentAimDirection { get; }
        public abstract Vector3 CurrentAimPosition { get; }

        protected virtual void HandleBeginAim()
        {
            OnBeginAiming.Invoke();
        }

        protected virtual void HandleEndAim()
        {
            OnEndAiming.Invoke();
        }

        public void RegisterBeginAimingCallback(UnityAction act) => OnBeginAiming.AddListener(act);
        public void UnregisterBeginAimingCallback(UnityAction act) => OnBeginAiming.RemoveListener(act);
        public void RegisterEndAimingCallback(UnityAction act) => OnEndAiming.AddListener(act);
        public void UnregisterEndAimingCallback(UnityAction act) => OnEndAiming.RemoveListener(act);
    }
}