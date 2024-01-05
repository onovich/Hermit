using UnityEngine;

namespace Polyperfect.War
{
    public abstract class ChangeableWatcher<C, T> : ChangeableBase<C> where C : Object, IChangeable<T>
    {
        protected C target
        {
            get => internalValue;
            set => TryChangeTo(value);
        }

        bool initialized = false;

        protected void OnEnable()
        {
            if (!target)
            {
                target = gameObject.GetComponentInParent<C>(); 
            }

            if (!target)
            {
                Debug.LogError($"No {typeof(C)} inheritor found on or in a parent or proxy of {name}.");
                enabled = false;
                return;
            }

            if (!initialized)
            {
                Initialize();
                RegisterChangeCallback(HandleTargetChanged);
                initialized = true;
            }

            HandleValueChange(target.GetInitializingEvent());
            target.RegisterChangeCallback(HandleValueChange);
        }

        protected void OnDisable()
        {
            if (target)
                target.UnregisterChangeCallback(HandleValueChange);
        }

        protected abstract void Initialize();
        protected abstract void HandleValueChange(ChangeEvent<T> e);

        void HandleTargetChanged(ChangeEvent<C> evt)
        {
            if (evt.Previous)
                evt.Previous.UnregisterChangeCallback(HandleValueChange);

            if (evt.Next)
            {
                evt.Next.RegisterChangeCallback(HandleValueChange);
                HandleValueChange(evt.Next.GetInitializingEvent());
            }
        }
    }
}