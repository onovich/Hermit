using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hermit {

    public static class TemplateInfra {

        public static async Task LoadAssets(TemplateInfraContext ctx) {
            var roles = await Addressables.LoadAssetsAsync<RoleTM>("TM_Role", null).Task;
            foreach (var role in roles) {
                ctx.Role_Add(role.typeID, role);
            }

            var tileTerrains = await Addressables.LoadAssetsAsync<TileTerrainTM>("TM_TileTerrain", null).Task;
            foreach (var tileTerrain in tileTerrains) {
                ctx.TileTerrain_Add(tileTerrain.typeID, tileTerrain);
            }
        }

    }

}