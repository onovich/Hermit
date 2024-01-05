using Polyperfect.Common;
using UnityEditor;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Usable))]
    public abstract class UseEffect : PolyMono, IUseEffect
    {
        public abstract void OnUse(UseContext context);

        [HighlightNull] [SerializeField] protected UsableCapability Capability;
        protected Usable usable;

        protected void Awake()
        {
            usable = GetComponent<Usable>();
            Initialize();
        }
        protected virtual void Initialize() { }
        void OnEnable() => usable.RegisterEffect(Capability, this);

        void OnDisable() => usable.UnregisterEffect(Capability, this);
    }
}