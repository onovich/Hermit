using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Damage_Receiver))]
    public class GrudgeHolder : PolyMono
    {
        public override string __Usage => $"Holds and updates grudge values for attackers. Used by {nameof(GrudgeTarget_AI)}.";
        
        readonly Dictionary<Shootable_Target, float> damageLookup = new Dictionary<Shootable_Target, float>();
        Damage_Receiver receiver;
        public float GrudgeReductionOverTime = 2f;
        public float MinThreshold = 10f;
        public float MaxThreshold = 50f;

        static readonly List<Shootable_Target> toRemove = new List<Shootable_Target>();
        public ITargetable GrudgeTarget => internalTarget;
        Shootable_Target internalTarget;
        float weight;
        void Awake()
        {
            receiver = GetComponent<Damage_Receiver>();
        }
        void Update()
        {
            toRemove.Clear();
            var keys = damageLookup.Keys.ToList();
            foreach (var item in keys)
            {
                var newValue = damageLookup[item] - Time.deltaTime * GrudgeReductionOverTime;
                damageLookup[item] = newValue;
                if (newValue <= 0f || item.AttachedHealth.IsDead)
                {
                    toRemove.Add(item);
                }
            }

            foreach (var item in toRemove)
            {
                damageLookup.Remove(item);
            }
            
            internalTarget = damageLookup.MaxBy(e => e.Value).Key;
            weight = CalculateWeight(internalTarget);
        }

        public float GetWeight() => weight;
        float CalculateWeight(Shootable_Target target)
        {
            if (target&&damageLookup.TryGetValue(internalTarget,out var val))
                return Mathf.InverseLerp(MinThreshold, MaxThreshold, val);
            return 0f;
        }

        protected void OnEnable() => receiver.RegisterDamageReceivedCallback(HandleDamageReceived);
        protected void OnDisable() => receiver.UnregisterDamageReceivedCallback(HandleDamageReceived);
        
        void HandleDamageReceived(DamageContext context)
        {
            var user = context.UseContext?.User;
            if (!user)
                return;
            var shootable = user.GetComponent<Shootable_Target>();
            if (!shootable)
                return;
            if (!damageLookup.ContainsKey(shootable))
                damageLookup.Add(shootable,0f);
            damageLookup[shootable] += context.DamageAmount;
        }
    }
}