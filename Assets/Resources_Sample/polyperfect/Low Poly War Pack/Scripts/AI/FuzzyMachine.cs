using System;
using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Polyperfect.War
{
    public class FuzzyMachine : PolyMono
    {
        public override string __Usage => $"Controls which AI components are active. Registers with a {nameof(Health_Reservoir)} if available";
        readonly Dictionary<FuzzyLayer, List<IFuzzyState>> components = new Dictionary<FuzzyLayer, List<IFuzzyState>>();

        public IReadOnlyDictionary<FuzzyLayer, List<IFuzzyState>> FuzzyStates => components;
        readonly Dictionary<FuzzyLayer, IFuzzyState> states = new  Dictionary<FuzzyLayer, IFuzzyState>();
        public IFuzzyState GetActiveState(FuzzyLayer layer)
        {
            if (states.TryGetValue(layer, out IFuzzyState ret))
            {
                Debug.Log($"Found result for {layer.ToString()}");
                return ret;
            }
            else
                return null;
        }

        public IEnumerable<IFuzzyState> GetActiveStates()
        {
            return states.Values;
        }

        
        [Serializable] public class StateEnterEvent : UnityEvent<IFuzzyState> { }

        public StateEnterEvent OnStateEntered;

        void Awake()
        {
            if (gameObject.TryGetComponent(out Health_Reservoir health))
            {
                health.RegisterDeathCallback(HandleDeath);
            }
        }

        void HandleDeath()
        {
            enabled = false;
        }

        void OnDisable()
        {
            DeactivateAll();
        }

        void DeactivateAll()
        {
            foreach (var item in components.SelectMany(category => category.Value)) 
                item.TryDeactivate();
        }

        public void RegisterFuzzyState(IFuzzyState component, FuzzyLayer layer)
        {
            if (!components.ContainsKey(layer))
                components.Add(layer, new List<IFuzzyState>());
            components[layer].Add(component);
        }

        public void UnregisterFuzzyState(IFuzzyState component, FuzzyLayer layer)
        {
            var type = layer;
            components[type].Remove(component);
            component.TryDeactivate();
        }

        public void Update()
        {
            foreach (var category in components)
            {
                var active = category.Value.MaxBy(c => c.GetWeight());
                if (active.GetWeight() <= 0)
                    active = default;
                foreach (var component in category.Value)
                {
                    if (component != active)
                        component.TryDeactivate();
                }
                states[category.Key] = active;
                var layerHasNewActiveState = active?.TryActivate();
                if (layerHasNewActiveState ?? false)
                {
                    OnStateEntered.Invoke(active);
                }
                    

                switch (active)
                {
                    case null:
                        continue;
                    case Object obj when obj:
                        active.ActiveUpdate();
                        break;
                    default:
                        active.ActiveUpdate();
                        break;
                }
            }
        }
    }
}