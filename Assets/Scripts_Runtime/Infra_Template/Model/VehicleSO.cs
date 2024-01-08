using UnityEngine;

namespace Hermit {
    [CreateAssetMenu(fileName = "SO_Vehicle", menuName = "Hermit/VehicleSO")]
    public class VehicleSO : ScriptableObject {
        public VehicleTM tm;
    }
}