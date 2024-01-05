using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public abstract class FailureConditionMonitor : PolyMono
    {
        public UnityEvent OnFailureConfirmed;
        protected abstract bool ConditionMet(UseFailedContext context);
        void Start()
        {
            foreach (var item in GetComponentsInParent<IUserEvents>()) 
                item.RegisterUseFailureCallback(act: i =>
                {
                    if (ConditionMet(i))
                    {
                        OnFailureConfirmed.Invoke();
                    }
                });
        }
    }
}