using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public class Spawner_UseEffect : UseEffect
    {
        public override string __Usage => $"Causes an object to be spawned when the attached {nameof(usable)} is used.";
        [SerializeField] protected GameObject ToSpawn;
        [SerializeField] protected Transform SpawnTransformOverride;
        [SerializeField] protected UnityEvent<GameObject> OnSpawn;
        public float AngleDeviation = 0f;
        Transform SpawnAtTransform => SpawnTransformOverride ? SpawnTransformOverride : transform;


        protected override void Initialize()
        {
            if (!ToSpawn)
            {
                enabled = false;
                throw new UnassignedReferenceException($"{nameof(ToSpawn)} prefab has not been assigned on {gameObject.name}");
            }
        }

        public override void OnUse(UseContext context)
        {
            var spawned = SpawnUtility.SpawnWithContext(ToSpawn, SpawnAtTransform.position, SpawnAtTransform.rotation, context);
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