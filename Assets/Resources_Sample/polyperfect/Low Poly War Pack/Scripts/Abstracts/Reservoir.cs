using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public abstract class Reservoir : PolyMono, IFractionalAmount,IChangeable
    {
        public abstract float FractionalAmount { get; }

        public abstract void RegisterChangeCallback(UnityAction act);
        public abstract void UnregisterChangeCallback(UnityAction act);
    }

    public abstract class Reservoir<T> : Reservoir,IAbsoluteAmount<T>,IExtractable<T>,IInsertable<T>,IChangeable<T>
    {
        [SerializeField] protected T currentAmount;
        [SerializeField] protected T maxAmount;
        [LowPriority][SerializeField] protected UnityEvent<ChangeEvent<T>> onChange;
        protected UnityEvent onChangeNoArgs = new UnityEvent();
        public T AbsoluteAmount => currentAmount;
        public T MaxAmount => maxAmount;

        /// <returns>Remainder after extracting all possible</returns>
        public T ExtractPossible(T amount) => doExtractPossible(amount);

        /// <returns>Remainder after extracting all possible</returns>
        protected abstract T doExtractPossible(T amount);
        public abstract bool CanFullyExtract(T amount);
        
        /// <returns>Remainder after inserting all possible</returns>
        public T InsertPossible(T amount)
        {
            var ret = doInsertPossible(amount);
            return ret;
        }

        /// <returns>Remainder after inserting all possible</returns>
        protected abstract T doInsertPossible(T amount);
        public abstract bool CanFullyInsert(T amount);

        protected void ChangeTo(T amount)
        {
            var previous = currentAmount;
            currentAmount = amount;
            onChange.Invoke(new ChangeEvent<T>(previous,currentAmount));
            onChangeNoArgs.Invoke();
        }
        public void RegisterChangeCallback(UnityAction<ChangeEvent<T>> act) => onChange.AddListener(act);
        public void UnregisterChangeCallback(UnityAction<ChangeEvent<T>> act) => onChange.RemoveListener(act);
        public ChangeEvent<T> GetInitializingEvent() => new ChangeEvent<T>(default, currentAmount);
        public override void RegisterChangeCallback(UnityAction act)
        {
            onChangeNoArgs.AddListener(act);
        }

        public override void UnregisterChangeCallback(UnityAction act)
        {
            onChangeNoArgs.RemoveListener(act);
        }
    }
}