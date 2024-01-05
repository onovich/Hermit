using Polyperfect.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Polyperfect.War
{
    public abstract class TargetableBase : PolyMono, ITargetable
    {
        [SerializeField] protected Transform LocationOverride;

        public Vector3 Position { get; private set; }//=> LocationOverride.position;
    

        protected void Awake()
        {
            if (!LocationOverride)
                LocationOverride = transform;
            Initialize();
        }

        protected abstract void Initialize();

        protected void Update()
        {
            Position = LocationOverride.position;
        }
        protected void OnEnable()
        {
            SceneTargetsManager.RegisterTargetable(this);
        }

        protected void OnDisable()
        {
            SceneTargetsManager.UnregisterTargetable(this);
        }
    }
}