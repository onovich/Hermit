using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(IUsableHolder))]
    public class AggressionTarget : ChangeableBase<ITargetable>
    {
        IUsableHolder holder;
        public override string __Usage => "Reference for the currently active target";

        [HighlightNull] [SerializeField] UsableCapability ShootCapability;
        [SerializeField] Transform EyePosition;

        public ITargetable Target
        {
            get => internalValue;
            set => TryChangeTo(value);
        }

        void Awake()
        {
            holder = GetComponent<IUsableHolder>();
            if (holder == null)
            {
                Debug.LogError($"{nameof(AggressionTarget)} on {gameObject.name} requires an {nameof(IUsableHolder)} to be attached.");
                enabled = false;
            }

        }

        void Update()
        {
            CanShootAndVisible = false;
            CanShoot = false;
            
            if (Target == null)
                return;

            if (!holder.CheckConditions(new UseContext(ShootCapability, gameObject), c => !(c is Cooldown_UseEffector)))
                return;
            CanShoot = true;
            
            var eyePos = EyePosition?EyePosition.position:transform.position;
            var targPos = Target.Position;
            if (Physics.Linecast(eyePos, targPos, out var hit) 
                && !hit.collider.GetComponentInParent<Damage_Receiver>()) 
                return;
            
            CanShootAndVisible = true;
        }
        public bool CanShoot { get; private set; }
        public bool CanShootAndVisible { get; private set; }
    }
}