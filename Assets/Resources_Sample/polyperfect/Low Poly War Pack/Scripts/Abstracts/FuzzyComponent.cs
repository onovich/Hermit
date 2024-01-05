using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [RequireComponent(typeof(FuzzyMachine))]
    public abstract class FuzzyComponent : PolyMono,IFuzzyState
    {
        protected abstract FuzzyLayer FuzzyLayer { get; }
        [SerializeField] [LowPriority] protected UnityEvent OnActivated;
        [SerializeField] [LowPriority] protected UnityEvent OnDeactivated;
        FuzzyMachine machine;
        [SerializeField] protected float WeightMultiplier = 1f;
        protected void Awake()
        {
            machine = GetComponent<FuzzyMachine>();
            Initialize();
        }

        public float GetWeight() => WeightMultiplier * CalculateWeight();
        protected abstract void Initialize();
        protected abstract float CalculateWeight();
        public abstract void ActiveUpdate();
        protected abstract void HandleActivate();
        protected abstract void HandleDeactivate();
        public bool IsActive { get; private set; }
        //enabled is used to toggle if the state is queried and run, while IsActive shows whether it's the currently selected behavior

        protected void OnEnable()
        {
            machine.RegisterFuzzyState(this,FuzzyLayer);
            OnEnabled();
        }

        protected void OnDisable()
        {
            machine.UnregisterFuzzyState(this,FuzzyLayer);
            OnDisabled();
        }

        protected void OnEnabled() { } //show toggles

        protected void OnDisabled() { }

        void OnActivate()
        {
            IsActive = true;
            HandleActivate();
            OnActivated.Invoke();
        }

        void OnDeactivate()
        {
            IsActive = false;
            HandleDeactivate();
            OnDeactivated.Invoke();
        }

        
        public bool TryActivate()
        {
            if (IsActive)
                return false;

            OnActivate();
            return true;
        }

        public bool TryDeactivate()
        {
            if (!IsActive)
                return false;
            
            OnDeactivate();
            return true;
        }
        
    }
}