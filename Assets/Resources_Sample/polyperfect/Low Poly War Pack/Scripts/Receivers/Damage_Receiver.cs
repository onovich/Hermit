using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public class Damage_Receiver : PolyMono, IDamageable<float>, IDamageable<DamageContext>
    {
        public override string __Usage => "Triggers damage events. Damage_Sender scripts interact with this automatically.";

        public float Sensitivity = 1f;
        [SerializeField] UnityEvent<DamageContext> OnDamaged = default;

        public void RegisterDamageReceivedCallback(UnityAction<DamageContext> action) => OnDamaged.AddListener(action);

        public void UnregisterDamageReceivedCallback(UnityAction<DamageContext> action) => OnDamaged.RemoveListener(action);

        public void TakeDamage(float damage) => OnDamaged.Invoke(new DamageContext {DamageAmount = damage*Sensitivity});

        public void TakeDamage(DamageContext damage)
        {
            damage.DamageAmount *= Sensitivity;
            OnDamaged.Invoke(damage);
        }
    }
}