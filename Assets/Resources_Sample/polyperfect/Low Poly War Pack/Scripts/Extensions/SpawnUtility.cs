using UnityEngine;

namespace Polyperfect.War
{
    public static class SpawnUtility
    {
        public static GameObject SpawnWithContext(GameObject prefab, Vector3 position, Quaternion rotation, UseContext context)
        {
            var spawned = Spawn(prefab, position, rotation);
            spawned.AddOrGetComponent<UseContext_Holder>().Receive(context);
            return spawned;
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) => Object.Instantiate(prefab, position,rotation);
    }
}