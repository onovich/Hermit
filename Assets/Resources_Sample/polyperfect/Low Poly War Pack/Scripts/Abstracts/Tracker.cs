using System.Collections.Generic;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public abstract class Tracker : PolyMono
    {
        public float Radius = 20f;
        public abstract void DoAddTargetable(ITargetable targetable);
        public abstract void DoRemoveTargetable(ITargetable targetable);
        public abstract bool ShouldTrack(ITargetable T);
        public abstract IEnumerable<ITargetable> TrackedTargetables { get; }
    }
    
    public abstract class Tracker<T> : Tracker where T:class,ITargetable
    {
        [SerializeField] protected  UnityEvent<T> OnTargetEnterRange = new UnityEvent<T>();
        [SerializeField] protected UnityEvent<T> OnTargetExitRange = new UnityEvent<T>();
        protected readonly HashSet<T> inRange = new HashSet<T>();
        public HashSet<T> Tracked => inRange;
        public override IEnumerable<ITargetable> TrackedTargetables => inRange;

        public override bool ShouldTrack(ITargetable targetable)
        {
            return targetable is T casted  && ShouldTrack(casted);
        }

        protected abstract bool ShouldTrack(T targetable);
        
        protected virtual void OnEnable() => SceneTargetsManager.RegisterSeeker(this);

        protected virtual void OnDisable() => SceneTargetsManager.UnregisterSeeker(this);
        public override void DoAddTargetable(ITargetable target)
        {
            inRange.Add((T)target);
            OnTargetEnterRange.Invoke((T)target);
        }
        
        public override void DoRemoveTargetable(ITargetable target)
        {
            inRange.Remove((T)target);
            OnTargetExitRange.Invoke((T)target);
        }

        public void RegisterTargetEnterRangeCallback(UnityAction<T> act) => OnTargetEnterRange.AddListener(act);

        public void UnregisterTargetEnterRangeCallback(UnityAction<T> act) => OnTargetEnterRange.RemoveListener(act);
        public void RegisterTargetExitRangeCallback(UnityAction<T> act) => OnTargetExitRange.AddListener(act);

        public void UnregisterTargetExitRangeCallback(UnityAction<T> act) => OnTargetExitRange.RemoveListener(act);
        public virtual Color GizmoColor => Color.white;
        
        void OnDrawGizmosSelected()
        {
            var col = Gizmos.color;
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireSphere(transform.position,Radius);
            Gizmos.color = col;
        }
    }
}