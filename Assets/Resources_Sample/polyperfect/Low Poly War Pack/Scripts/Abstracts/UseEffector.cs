using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Usable))]
    public abstract class UseEffector : PolyMono, IUseEffect, IUseCondition
    {
        [HighlightNull] [SerializeField] protected UsableCapability Capability;
        protected Usable usable;

        protected void Awake()
        {
            usable = GetComponent<Usable>();
            Initialize();
        }

        public abstract void OnUse(UseContext context);

        protected virtual void Initialize()
        {
        }

        public abstract bool CheckCondition(UseContext context, out Reason reason);


        protected void OnEnable()
        {
            usable.RegisterEffect(Capability, this);
            usable.RegisterCondition(Capability, this);
        }
        
        protected void OnDisable()
        {
            usable.UnregisterEffect(Capability, this);
            usable.UnregisterCondition(Capability, this);
        }
    }
}