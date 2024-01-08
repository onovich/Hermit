using UnityEngine;

namespace Hermit {
    [CreateAssetMenu(fileName = "SO_Role", menuName = "Hermit/RoleSO")]
    public class RoleSO : ScriptableObject {
        public RoleTM tm;
    }
}