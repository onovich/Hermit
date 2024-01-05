using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(InputCollector))]
    public abstract class CapabilityUser : PolyMono
    {
        protected IUser holder;
        protected InputCollector input;
        [SerializeField] protected UsableCapability Capability;

        void Awake()
        {
            input = GetComponent<InputCollector>();
            holder = GetComponent<IUser>();
            if (!(holder as MonoBehaviour))
            {
                Debug.LogError($"{GetType().Name} on {gameObject.name} requires an {nameof(IUser)}. Disabling.");
                enabled = false;
            }
        }

        protected void Update()
        {
            if (CheckCondition())
                holder.Use(Capability);
        }

        protected abstract bool CheckCondition();
    }
}