using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public class Cooldown_UseEffector : UseEffector,IFractionalAmount
    {
        public override string __Usage => $"Makes the attached {nameof(usable)} have a cooldown between uses.";
        public float CooldownDuration = .5f;
        [SerializeField] UnityEvent OnCooldownComplete;
        [SerializeField] UnityEvent OnCooldownBegin;

        float coolingValue;
        bool cooldownComplete;
        public bool IsCooledDown => cooldownComplete;

        protected override void Initialize()
        {
            cooldownComplete = true;
            coolingValue = CooldownDuration;
        }


        void Update()
        {
            //an update based method is used instead of just storing the use time or coroutine to avoid shenanigans with disabled behaviors and such
            if (IsCooledDown) //OnEnable and OnDisable are used for enabling and disabling the condition, so this (although not ideal) is used
                return;
            
            coolingValue += Time.deltaTime;
            if (coolingValue >= CooldownDuration)
                HandleCooldownComplete();
        }

        void HandleCooldownComplete()
        {
            cooldownComplete = true;
            OnCooldownComplete.Invoke();
        }

        public static readonly Reason NotCooledDown = new Reason();
        public override bool CheckCondition(UseContext context, out Reason reason)
        {
            reason = IsCooledDown ? Reason.None : NotCooledDown;
            return IsCooledDown;
        }

        public override void OnUse(UseContext context)
        {
            coolingValue = 0f;
            cooldownComplete = false;
            OnCooldownBegin.Invoke();
        }

        public void RegisterCooldownBeginCallback(UnityAction act) => OnCooldownBegin.AddListener(act);
        public void UnregisterCooldownBeginCallback(UnityAction act) => OnCooldownBegin.RemoveListener(act);
        public void RegisterCooldownCompleteCallback(UnityAction act) => OnCooldownComplete.AddListener(act);
        public void UnregisterCooldownCompleteCallback(UnityAction act) => OnCooldownComplete.RemoveListener(act);
        public float FractionalAmount => Mathf.Clamp01(coolingValue / CooldownDuration);
    }
}