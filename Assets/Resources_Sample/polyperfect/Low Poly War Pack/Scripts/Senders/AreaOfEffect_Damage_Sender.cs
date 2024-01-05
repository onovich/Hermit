using System;
using Polyperfect.Common;

namespace Polyperfect.War
{
    public class AreaOfEffect_Damage_Sender : PolyMono
    {
        public override string __Usage => "Simply damages things within its reach.";
        public bool ActivateOnStart = true;
        
        public float DamageAmount = 50f;
        public float ImpactStrength = 10f;
        public float Radius = 5f;

        void Start()
        {
            if (ActivateOnStart)
                SendDamage();
        }

        public void SendDamage()
        {
            DamageUtility.AoEDamage(null, transform.position, Radius, DamageAmount, ImpactStrength);
        }
    }
}