using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [RequireComponent(typeof(DynamicUsableHolder))]
    [RequireComponent(typeof(InputCollector))]
    public class Usable_Carrier : PolyMono
    {
        [SerializeField] List<Usable> StartingWeapons;
        [SerializeField] UnityEvent<ChangeEvent<IEnumerable<Usable>>> OnCollectionUpdated = default;
        DynamicUsableHolder _usableHolder;

        readonly Dictionary<Usable, Usable> usablePrefabToInstanceLookup = new Dictionary<Usable, Usable>();
        InputCollector inputs;
        readonly List<Usable> usableInstances = new List<Usable>();
        public IEnumerable<Usable> UsableInstances => usableInstances;
        void Awake()
        {
            _usableHolder = GetComponent<DynamicUsableHolder>();
            inputs = GetComponent<InputCollector>();
        }

        void Start()
        {
            RegisterChangeCallback((c)=>
            {
                var index = c.Next?.Count()-1 ?? 0;
                SetUsableIndex(index);
            });
            SetUpUsables();
            inputs[Inputs.Next].OnActivate += ()=>IncrementUsableIndex(true);
            inputs[Inputs.Prev].OnActivate += ()=>IncrementUsableIndex(false);
        }

        void IncrementUsableIndex(bool up)
        {
            if (usableInstances.Count <= 0)
                return;
            var currentIndex = usableInstances.IndexOf(_usableHolder.Usable);
            if (up)
                currentIndex++;
            else
                currentIndex--;

            if (currentIndex < 0)
                currentIndex = usableInstances.Count - 1;
            else if (currentIndex >= usableInstances.Count)
                currentIndex = 0;
            SetUsableIndex(currentIndex);
        }

        void SetUpUsables()
        {
            foreach (var weaponType in StartingWeapons) AddUsableWithoutNotify(weaponType);
            
            OnCollectionUpdated.Invoke(new ChangeEvent<IEnumerable<Usable>>(null,usablePrefabToInstanceLookup.Keys));
        }

        public bool HasUsable(Usable usablePrefab) => usablePrefabToInstanceLookup.ContainsKey(usablePrefab);

        public void AddUsable(Usable usablePrefab)
        {
            var prevValues = usablePrefabToInstanceLookup.Keys.ToList();
            AddUsableWithoutNotify(usablePrefab);
            OnCollectionUpdated.Invoke(new ChangeEvent<IEnumerable<Usable>>(prevValues,usablePrefabToInstanceLookup.Keys));
        }

        void AddUsableWithoutNotify(Usable usablePrefab)
        {
            if (!usablePrefab)
                Debug.LogError(($"UsablePrefab was null on {gameObject.name}"));
            if (HasUsable(usablePrefab))
            {
                Debug.LogWarning($"{gameObject.name} already had {usablePrefab.name}");
                return;
            }

            var createdUsable = _usableHolder.CreateUsable(usablePrefab.gameObject);
            usablePrefabToInstanceLookup.Add(usablePrefab, createdUsable);
            usableInstances.Add(createdUsable);
            createdUsable.gameObject.SetActive(false);
        }

        void SetUsableIndex(int index)
        {
            var useNull = index < 0 || index >= usableInstances.Count;
            _usableHolder.Usable = useNull?null:usableInstances[index];
        }

        public void RegisterChangeCallback(UnityAction<ChangeEvent<IEnumerable<Usable>>> onChange)
        {
            OnCollectionUpdated.AddListener(onChange);
        }

        public void UnregisterChangeCallback(UnityAction<ChangeEvent<IEnumerable<Usable>>> onChange)
        {
            OnCollectionUpdated.RemoveListener(onChange);
        }

        public ChangeEvent<IEnumerable<Usable>> GetInitializingEvent() => new ChangeEvent<IEnumerable<Usable>>(null, usablePrefabToInstanceLookup.Keys);
        public override string __Usage => $"Holds all {nameof(Usable)}s that the entity has access to.";
    }
}  