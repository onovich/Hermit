using System;
using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public class Ammo_Carrier : PolyMono
    {
        public override string __Usage => "Container for all ammo carried by an entity.";

        [Serializable]
        struct AmmoStore
        {
            public AmmoType Type;
            public int Count;
            public int MaxAmount;
        }

        [SerializeField] List<AmmoStore> StartingAmmo = new List<AmmoStore>();
        readonly Dictionary<AmmoType, int> ammos = new Dictionary<AmmoType, int>();
        public IEnumerable<AmmoType> NonzeroContainedTypes => ammos.Where(e => e.Value > 0).Select(e => e.Key).ToList();

        void Awake()
        {
            foreach (var item in StartingAmmo)
            {
                ammos.Add(item.Type, item.Count);
            }
        }

        public bool SupportsAmmo(AmmoType type) => ammos.ContainsKey(type);

        public int GetRoundCount(AmmoType type) => SupportsAmmo(type) ? ammos[type] : 0;

        public bool TryAddRounds(AmmoType type, int rounds)
        {
            if (!SupportsAmmo(type))
                return false;

            ammos[type] = ammos[type] + rounds;
            return true;
        }

        public void TryAddAmmoSupport(AmmoType type)
        {
            if (!ammos.ContainsKey(type))
                ammos.Add(type, 0);
        }

        public int TakeRounds(AmmoType type, int maxCount)
        {
            if (!ammos.ContainsKey(type))
                return 0;
            var takeOut = Mathf.Min(maxCount, GetRoundCount(type));
            ammos[type] -= takeOut;
            return takeOut;
        }
    }
}