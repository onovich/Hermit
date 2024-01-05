using System.Collections;
using Polyperfect.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Polyperfect.War
{
    public class RandomSpawner : PolyMono
    {
        public override string __Usage => "Spawns prefabs randomly.";
        [HighlightNull] public GameObject Prefab;
        public float SpawnRadius = 3f;
        [Range(0, 15f)] public float MinDelay = 2f;
        [Range(0, 15f)] public float MaxDelay = 5f;
        public int MaxSpawnCount = 1000000;

        void OnValidate()
        {
            MaxDelay = Mathf.Max(MinDelay, MaxDelay);
            MaxSpawnCount = Mathf.Max(0, MaxSpawnCount);
        }

        void Start()
        {
            StartCoroutine(spawnCoroutine());
        }

        IEnumerator spawnCoroutine()
        {
            for(var i= 0; i < MaxSpawnCount; i++)
            {
                yield return new WaitForSeconds(Random.Range(MinDelay, MaxDelay));
                Instantiate(Prefab, transform.position + Vector3.Scale(new Vector3(1f, 0f, 1f), Random.onUnitSphere * SpawnRadius), transform.rotation);
            }
        }

    }
}