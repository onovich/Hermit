using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Hermit {

    public class AssetsInfraContext {

        public AsyncOperationHandle entityHandle;
        Dictionary<string, GameObject> entityAssets;

        public AssetsInfraContext() {
            entityAssets = new Dictionary<string, GameObject>();
        }

        public void Entity_Add(string key, GameObject asset) {
            entityAssets.Add(key, asset);
        }

        GameObject Entity_Get(string key) {
            var has = entityAssets.TryGetValue(key, out var asset);
            if (!has) {
                HLog.LogError($"Entity_Get: {key} not found");
            }
            return asset;
        }

        public GameObject Entity_GetRole() {
            return Entity_Get("GO_Role");
        }

        public GameObject Entity_GetVehicle() {
            return Entity_Get("GO_Vehicle");
        }

        public GameObject Entity_GetBullet() {
            return Entity_Get("GO_Bullet");
        }

    }

}