using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public class Health_Reservoir : FloatReservoir
    {
        public override string __Usage => "Keeps track of health, and sends a Death event if health drops to 0. Automatically registers with a " + nameof(Damage_Receiver) + " if one is present.";

        public float MaxHealth => maxAmount;
        public float HealthFraction => Health / MaxHealth;
        public bool IsDead => Health <= 0f;
        public bool IsAlive => Health > 0f;
        [SerializeField] UnityEvent OnDeath = default;
        [SerializeField] UnityEvent OnRevive = default;

        void Reset()
        {
            currentAmount = 100;
            maxAmount = 100;
        }

        public float Health
        {
            get => currentAmount;
            set
            {
                var intendedNew = Mathf.Clamp(value, 0f, MaxHealth);
                if (!Mathf.Approximately(intendedNew,currentAmount))
                    ChangeTo(intendedNew);
            }
        }

        void Start()
        {
            RegisterChangeCallback((f) =>
            {
                if (f.Next<=0)
                    HandleDeath();
                if (f.Previous <= 0 && f.Next > 0)
                    HandleRevive();
            });
            
            var damageable = GetComponent<Damage_Receiver>();
            if (damageable)
                damageable.RegisterDamageReceivedCallback(ReceiveDamage);
            var respawner = GetComponent<Respawner>();
            if (respawner)
                respawner.RegisterRespawnCallback(HandleRespawn);
            RegisterDeathCallback(StopHealRoutines);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
                Health += 10f;
        }

        void HandleRespawn() => Health = maxAmount;

        void HandleRevive() => OnRevive.Invoke();

        void HandleDeath() => OnDeath.Invoke();

        void ReceiveDamage(DamageContext args) => Health = (Health-args.DamageAmount);

        public void RegisterDeathCallback(UnityAction action) => OnDeath.AddListener(action);

        public void UnregisterDeathCallback(UnityAction action) => OnDeath.RemoveListener(action);

        public void RegisterReviveCallback(UnityAction action) => OnRevive.AddListener(action);
        public void UnregisterReviveCallback(UnityAction action) => OnRevive.RemoveListener(action);
        
        public void UseMedkit(Medkit_Pickup medkit)
        {
            if (medkit.IsInstant) 
                Health += medkit.Amount;
            else
            {
                var reference = new CoroutineReference();
                var routine = StartCoroutine(HealOverTime(medkit.Duration, medkit.Amount,reference));
                reference.Coroutine = routine;
                healOverTimes.Add(reference);
            }
        }

        readonly List<CoroutineReference> healOverTimes = new List<CoroutineReference>();

        void StopHealRoutines()
        {
            foreach (var item in healOverTimes) 
                StopCoroutine(item.Coroutine);
            
            healOverTimes.Clear();
        }
        
        IEnumerator HealOverTime(float duration, float totalAmount,CoroutineReference reference)
        {
            var healRemaining = totalAmount;
            var lastTime = Time.time;
            while (healRemaining>0f)
            {
                var maxHealThisFrame = totalAmount * (Time.time - lastTime) / duration;
                var toHeal = Mathf.Min(healRemaining, maxHealThisFrame);
                lastTime = Time.time;
                healRemaining -= toHeal;
                Health += toHeal;
                yield return null;
            }
            yield return null; //to prevent shenanigans with same frame before stopping
            healOverTimes.Remove(reference);
        }
    }
}
