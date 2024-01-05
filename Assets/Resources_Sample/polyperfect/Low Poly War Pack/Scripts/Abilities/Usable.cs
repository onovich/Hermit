using System;
using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public class Usable : PolyMono,IUseChecker
    {
        public override string __Usage => "Allows registering use events and use conditions, such as ammo constraints and spawning behaviors";

        public string Name;
        public List<UsableCapability> Capabilities = new List<UsableCapability>();
        protected readonly Dictionary<UsableCapability, HashSet<IUseCondition>> abilityConditions = new Dictionary<UsableCapability, HashSet<IUseCondition>>();

        [SerializeField] protected List<UseEventData> UseEvents = new List<UseEventData>();
        
        readonly Dictionary<UsableCapability, List<IUseEffect>> useEffectLookup = new Dictionary<UsableCapability, List<IUseEffect>>();
        readonly Dictionary<UsableCapability, UnityEvent<UseContext>> useEventLookup = new Dictionary<UsableCapability, UnityEvent<UseContext>>();
        readonly Dictionary<UsableCapability, UnityEvent<UseFailedContext>> useFailedEventLookup = new Dictionary<UsableCapability, UnityEvent<UseFailedContext>>();
        [SerializeField] UnityEvent<UseContext> OnUse;

        [System.Serializable]
        public struct UseEventData
        {
            [DisableInInspector] public UsableCapability Capability;
            public UnityEvent<UseContext> OnUse;
            public UnityEvent<UseFailedContext> OnFailedUse;
        }

        void OnValidate()
        {
            //ensure no repeats in Capability list
            for (var i = 0; i < Capabilities.Count; i++)
            for (var j = i + 1; j < Capabilities.Count; j++)
                if (Capabilities[i] == Capabilities[j])
                    Capabilities[j] = null;
            Capabilities = Capabilities.Distinct().ToList();

            //Make sure events are in same order as capabilities
            var useEventDictionary = UseEvents.ToDictionary(item => item.Capability);
            UseEvents.Clear();
            foreach (var item in Capabilities.Where(c => c))
                UseEvents.Add(useEventDictionary.ContainsKey(item) ? useEventDictionary[item] : new UseEventData() {Capability = item});
        }

        bool initialized;

        void Awake()
        {
            TryInitialize();
        }

        void TryInitialize()
        {
            if (initialized)
                return;
            foreach (var item in Capabilities)
                if (item)
                    abilityConditions.Add(item, new HashSet<IUseCondition>());
            foreach (var item in UseEvents)
            {
                useEffectLookup.Add(item.Capability,new List<IUseEffect>());
            }

            foreach (var item in UseEvents)
            {
                useEventLookup.Add(item.Capability, item.OnUse);
                useFailedEventLookup.Add(item.Capability,item.OnFailedUse);
            }

            initialized = true;
        }

        public void RegisterCondition(UsableCapability ability, IUseCondition condition) //Func<UseContext,bool> condition)
        {
            TryInitialize();
            Debug.Assert(abilityConditions.ContainsKey(ability), $"{nameof(Usable)} on {gameObject.name} does not have capability {ability.name} assigned.");
            abilityConditions[ability].Add(condition);
        }

        public void UnregisterCondition(UsableCapability ability, IUseCondition condition) => abilityConditions[ability].Remove(condition);

        public void RegisterEffect(UsableCapability ability, IUseEffect effect)
        {
            TryInitialize();
            
            
            if (useEffectLookup.TryGetValue(ability, out var list)) 
                list.Add(effect);
            else
            {
                Debug.LogError($"Tried adding {ability.name} effect to {gameObject.name}, which it doesn't support");
            }
        }

        public void UnregisterEffect(UsableCapability ability, IUseEffect effect)
        {
            useEffectLookup[ability].Remove(effect);
        }

        public static readonly Reason UnsupportedCapability = new Reason();
        public bool TryUse(UseContext context, out UseFailedContext failedCondition)
        {
            failedCondition = default;
            if (!enabled)
                return false;
            if (!HasCapability(context.Capability))
            {
                failedCondition = new UseFailedContext(null, UnsupportedCapability);
                return false;
            }

            if (!CheckCondition(context, out var failedOn))
            {
                useFailedEventLookup[context.Capability].Invoke(failedOn);
                failedCondition = failedOn;
                return false;
            }

            foreach (var item in useEffectLookup[context.Capability])
                item.OnUse(context);
            useEventLookup[context.Capability].Invoke(context);
            
            OnUse.Invoke(context);
            return true;
        }

        public bool CanUse(UseContext context)
        {
            return enabled && CheckCondition(context, out _);
        }

        public bool CheckConditions(UseContext context, Func<IUseCondition, bool> conditionSelector)
        {
            return abilityConditions[context.Capability].Where(conditionSelector).All(c => c.CheckCondition(context,out _));
        }

        bool HasCapability(UsableCapability capability) => useEffectLookup.ContainsKey(capability);
        
        protected virtual bool CheckCondition(UseContext context,out UseFailedContext failedOn)
        {
            failedOn = default;
            if (!HasCapability(context.Capability))
                return false;
            foreach (var item in abilityConditions[context.Capability])
            {
                if (!item.CheckCondition(context, out var reason))
                {
                    failedOn = new UseFailedContext(item,reason);
                    return false;
                }
            }

            return true;
        }

        public void RegisterUseCallback(UnityAction<UseContext> act)
        {
            TryInitialize();
            OnUse.AddListener(act);
        }

        public void UnregisterUseCallback(UnityAction<UseContext> act)
        {
            TryInitialize();
            OnUse.RemoveListener(act);
        }
    }
}