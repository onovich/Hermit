using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Ammo_Reservoir))]
    public class Reload_UseEffector : UseEffector
    {
        public override string __Usage => $"Standard behavior for reloading. The User must have an {nameof(Ammo_Carrier)} to extract from.";
        public float ReloadDuration = 1f;
        [SerializeField] UnityEvent<UseContext> OnBegin;
        [SerializeField] UnityEvent<UseContext> OnEnd;
        [SerializeField] UnityEvent OnCancel;
        bool isReloading;
        Ammo_Reservoir ammo;
        const float reloadCooldown = .2f;
        protected override void Initialize() => ammo = GetComponent<Ammo_Reservoir>();
        public static readonly Reason AtMaxAmmo = new Reason(), AlreadyReloading = new Reason(), NoAvailableAmmo = new Reason();

        public override bool CheckCondition(UseContext context, out Reason reason)
        {
            var owningCarrier = context.User.GetComponent<Ammo_Carrier>();
            
            if (isReloading)
                reason = AlreadyReloading;
            else if (ammo.IsAtMax())
                reason = AtMaxAmmo;
            else if (owningCarrier.GetRoundCount(ammo.AmmoType) <= 0)
                reason = NoAvailableAmmo;
            else
                reason = Reason.None;

            return reason == Reason.None;
        }

        protected new void OnEnable()
        {
            isReloading = false;
            base.OnEnable();
        }
        
        protected new void OnDisable()
        {
            StopAllCoroutines();
            if (isReloading)
                OnCancel.Invoke();
            isReloading = false;
            base.OnDisable();
        }
        
        
        public override void OnUse(UseContext context)
        {
            if (!context.User || !context.User.GetComponent<Ammo_Carrier>())
            {
                Debug.LogError($"For reloading to work, the user must have an {nameof(Ammo_Carrier)} to extract from.");
                return;
            }
            StopAllCoroutines();
            StartCoroutine(reloadRoutine(context));
        }
    
        IEnumerator reloadRoutine(UseContext context)
        {
            isReloading = true;
            if (!context.User)
            {
                isReloading = false;
                yield break;
            }

            
            OnBegin.Invoke(context);
            var library = context.User.GetComponent<Ammo_Carrier>();
            var extracted = ammo.ExtractAll();
            library.TryAddRounds(extracted.AmmoType, extracted.Amount);
            yield return new WaitForSeconds(ReloadDuration);
            ammo.InsertPossible(library.TakeRounds(ammo.AmmoType, ammo.MaxAmount));
            OnEnd.Invoke(context);
            yield return new WaitForSeconds(reloadCooldown);
            isReloading = false;
        }

        public void RegisterBeginReloadCallback(UnityAction<UseContext> act) => OnBegin.AddListener(act);
        public void UnregisterBeginReloadCallback(UnityAction<UseContext> act) => OnBegin.RemoveListener(act);
        public void RegisterEndReloadCallback(UnityAction<UseContext> act) => OnEnd.AddListener(act);
        public void UnregisterEndReloadCallback(UnityAction<UseContext> act) => OnEnd.RemoveListener(act);
        public void RegisterCancelReloadCallback(UnityAction act) => OnCancel.AddListener(act);
        public void UnregisterCancelReloadCallback(UnityAction act) => OnCancel.RemoveListener(act);
    }
}