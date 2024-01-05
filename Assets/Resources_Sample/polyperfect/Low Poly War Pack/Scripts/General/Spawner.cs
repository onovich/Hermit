using System;
using Polyperfect.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Polyperfect.War
{
    public class Spawner : PolyMono
    {
        public override string __Usage =>
            $"Easily spawn objects. One random one from within {nameof(PossibleSpawns)} is selected. If holder or ancestor has a {nameof(UseContext_Holder)}, it will automatically be sent to {nameof(IUseContextReceiver)}s on the spawned object.";
        [SerializeField] GameObject[] PossibleSpawns;

        GameObject GetToSpawn()
        {
            if (PossibleSpawns.Length <= 0)
                throw new Exception($"Spawner on {(gameObject ? gameObject.name : "NULL")} must have at least one possible prefab to spawn.");
            return PossibleSpawns[Random.Range(0, PossibleSpawns.Length)];
        }
        Transform targetTransform => transform;
        public void Spawn()
        {
            TrySpawnWithContext(targetTransform.position,targetTransform.rotation);
        }
        GameObject TrySpawnWithContext(Vector3 position, Quaternion rotation)
        {
            var thisContextHolder = gameObject.GetComponentInParent<UseContext_Holder>();

            return thisContextHolder 
                ? SpawnUtility.SpawnWithContext(GetToSpawn(), position, rotation,thisContextHolder.Context) 
                : SpawnUtility.Spawn(GetToSpawn(), position, rotation);
        }

        public void SpawnAfter(float delay)
        {
            Invoke(nameof(Spawn),delay);
        }

        public void Spawn(Collision collision)
        {
            var hasContact = collision.contactCount > 0;
            if (!hasContact)
            {
                Debug.LogError($"{gameObject.name} is trying to spawn based on a collision without contacts");
                return;
            }

            var contact = collision.GetContact(0);
            var forward = collision.relativeVelocity;
            if (forward.sqrMagnitude <= .000001f)
                forward = contact.thisCollider.transform.forward;
            var up = contact.normal;
            Vector3.OrthoNormalize(ref up,ref forward);
            var orientation = Quaternion.LookRotation(forward, up);
            TrySpawnWithContext(contact.point, orientation); 
        }

        public GameObject Spawn(UseContext context)
        {
            var spawned = TrySpawnWithContext(targetTransform.position,targetTransform.rotation);
            foreach (var item in spawned.GetComponents<IUseContextReceiver>())
            {
                item.Receive(context);
            }

            return spawned;
        }
    }
}