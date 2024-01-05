using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Damage_Receiver))]
    public class Knockback : PolyMono
    {
        public float KnockbackMultiplier = 1f;
        public override string __Usage => "Applies knockback when damaged.";

        Damage_Receiver damage;
        Rigidbody body;
        void Awake()
        {
            damage = GetComponent<Damage_Receiver>();
            body = GetComponent<Rigidbody>();
        }

        void OnEnable() => damage.RegisterDamageReceivedCallback(HandleDamageReceived);
        void OnDisable() => damage.UnregisterDamageReceivedCallback(HandleDamageReceived);
        void HandleDamageReceived(DamageContext context) => body.AddForceAtPosition(context.ImpactVector * KnockbackMultiplier, context.ImpactPosition,ForceMode.Impulse);
    }
}