using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class OnCollide_Damage_Sender : PolyMono
    {
        public override string __Usage => "Sends a damage event upon physically colliding with an object.";
        public float Damage = 10;
        void OnCollisionEnter(Collision other)
        {
            var damageable = other.gameObject.GetComponentInParent<Damage_Receiver>();
            if (!damageable)
                return;

            damageable.TakeDamage(Damage);
        }
    }
}