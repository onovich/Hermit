using System;
using System.Collections;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [System.Serializable]
    public abstract class ChangeableBase<T> : PolyMono, IChangeable<T>
    {
        protected T internalValue;
        [LowPriority] [SerializeField] protected UnityEvent<ChangeEvent<T>> OnChanged = default;
        UnityEvent<ChangeEvent<T>> onChangedHighPriority = new UnityEvent<ChangeEvent<T>>();


        protected void TryChangeTo(T newValue)
        {
            var bothNull = newValue == null && internalValue == null;
            if (bothNull||(newValue != null&&newValue.Equals(internalValue)))
                return;
            
            var arg = new ChangeEvent<T>(internalValue, newValue); //TODO: this could be improved in the future using Pooling
            internalValue = newValue;
            onChangedHighPriority.Invoke(arg);
            OnChanged.Invoke(arg);
        }

        public void RegisterChangeCallback(UnityAction<ChangeEvent<T>> action) => onChangedHighPriority.AddListener(action);
        public void UnregisterChangeCallback(UnityAction<ChangeEvent<T>> action) => onChangedHighPriority.RemoveListener(action);

        public ChangeEvent<T> GetInitializingEvent() => new ChangeEvent<T>(default, internalValue);
    }
}