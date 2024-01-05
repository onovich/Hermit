using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public class MultiSpawner_UseEffect : UseEffect
    {
        public override string __Usage => $"Causes an object to be spawned when the attached {nameof(usable)} is used. Cycles through the specified {nameof(SpawnLocations)}";
        [SerializeField] protected GameObject ToSpawn;
        [SerializeField] Transform[] SpawnLocations;
        [SerializeField] protected UnityEvent<GameObject> OnSpawn;
        public float AngleDeviation = 2f;
        int spawnIndex;
        protected override void Initialize()
        {
            if (!ToSpawn)
            {
                enabled = false;
                throw new UnassignedReferenceException($"{nameof(ToSpawn)} prefab has not been assigned on {gameObject.name}");
            }
        }

        void IncrementIndex()
        {
            spawnIndex++;
            spawnIndex %= SpawnLocations.Length;
        }
        public override void OnUse(UseContext context)
        {
            var spawned = SpawnUtility.SpawnWithContext(ToSpawn, SpawnLocations[spawnIndex].position, SpawnLocations[spawnIndex].rotation, context);
            IncrementIndex();
            
            var roll = Random.Range(0f, 360f);
            var incline = Random.Range(0f, AngleDeviation);
            var vect = Vector3.RotateTowards(Vector3.forward, Vector3.up, incline * Mathf.Deg2Rad, 0f);
            var newrot = Quaternion.AngleAxis(roll, Vector3.forward)*vect;
            
            spawned.transform.rotation *= Quaternion.LookRotation(newrot, Vector3.up); 
            OnSpawn.Invoke(spawned);
        }

        public void RegisterSpawnCallback(UnityAction<GameObject> act) => OnSpawn.AddListener(act);
        public void UnregisterSpawnCallback(UnityAction<GameObject> act) => OnSpawn.RemoveListener(act);
    }
}