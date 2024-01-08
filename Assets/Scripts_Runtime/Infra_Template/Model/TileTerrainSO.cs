using UnityEngine;

namespace Hermit {
    [CreateAssetMenu(fileName = "SO_TileTerrain", menuName = "Hermit/TileTerrainSO")]
    public class TileTerrainSO : ScriptableObject {
        public TileTerrainTM tm;
    }
}