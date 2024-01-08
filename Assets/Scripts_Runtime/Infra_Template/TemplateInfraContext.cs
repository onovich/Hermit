using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public class TemplateInfraContext {

        Dictionary<int, RoleTM> roles;
        Dictionary<int,TileTerrainTM> tileTerrains;

        public TemplateInfraContext() {
            roles = new Dictionary<int, RoleTM>();
            tileTerrains = new Dictionary<int, TileTerrainTM>();
        }

        // Role
        public void Role_Add(int id, RoleTM role) {
            roles.Add(id, role);
        }

        public bool Role_TryGet(int id, out RoleTM role) {
            return roles.TryGetValue(id, out role);
        }

        // TileTerrain
        public void TileTerrain_Add(int id, TileTerrainTM tileTerrain) {
            tileTerrains.Add(id, tileTerrain);
        }

        public bool TileTerrain_TryGet(int id, out TileTerrainTM tileTerrain) {
            return tileTerrains.TryGetValue(id, out tileTerrain);
        }

    }

}