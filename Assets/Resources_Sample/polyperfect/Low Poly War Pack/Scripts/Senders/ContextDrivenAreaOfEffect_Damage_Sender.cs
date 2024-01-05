using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class ContextDrivenAreaOfEffect_Damage_Sender : PolyMono,IUseContextReceiver
    {
        public override string __Usage => $"Sends a damage event for everything in range.\nActivated immediately upon receiving a {nameof(UseContext)} from another script, such as a {nameof(Spawner)}.";
        
        public float DamageAmount = 50f;
        public float ImpactStrength = 10f;
        public float Radius = 5f;

        public void Receive(UseContext context) => DamageUtility.AoEDamage(context, transform.position, Radius, DamageAmount, ImpactStrength);

        void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position,Radius);
    }
}