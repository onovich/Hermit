using System.Collections.Generic;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Usable_Carrier))]
    public class RandomUsableGiver : PolyMono
    {
        public override string __Usage => $"Gives the holder a random starting {nameof(Usable)} on start. Will also give ammo if an {nameof(Ammo_Carrier)} is attached.";
        public List<Usable> PossiblePrefabs;
        public int GiftedAmmo = 1000;
        void Start()
        {
            if (PossiblePrefabs.Count <= 0)
            {
                Debug.LogWarning($"{nameof(PossiblePrefabs)} has no elements on object {name}");
                return;
            }

            var library = GetComponent<Usable_Carrier>();
            var prefab = PossiblePrefabs[Random.Range(0, PossiblePrefabs.Count)];
            library.AddUsable(prefab);
            if (TryGetComponent(out Ammo_Carrier ammoLibrary))
            {
                if (prefab.TryGetComponent(out Ammo_Reservoir ammoReservoir))
                {
                    ammoLibrary.TryAddAmmoSupport(ammoReservoir.AmmoType);
                    ammoLibrary.TryAddRounds(ammoReservoir.AmmoType, GiftedAmmo);
                }
            }
        }
    }
}