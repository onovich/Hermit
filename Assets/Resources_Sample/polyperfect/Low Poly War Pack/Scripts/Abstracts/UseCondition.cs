using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Usable))]
    public abstract class UseCondition : PolyMono, IUseCondition
    {
        protected Usable usable;
        [HighlightNull] [SerializeField] protected UsableCapability Capability;

        
        protected void Awake()
        {
            usable = GetComponent<Usable>();
            Initialize();
        }
        protected virtual void Initialize() { }

        void OnEnable() => usable.RegisterCondition(Capability, this);

        void OnDisable() => usable.UnregisterCondition(Capability, this);

        public abstract bool CheckCondition(UseContext context,out Reason reason);
    }
}