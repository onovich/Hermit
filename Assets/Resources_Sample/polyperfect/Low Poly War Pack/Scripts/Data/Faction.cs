using System.Collections.Generic;
using UnityEngine;

namespace Polyperfect.War
{
    [CreateAssetMenu]
    public class Faction:ScriptableObject
    {
        public List<Faction> Hostiles;
    }
}