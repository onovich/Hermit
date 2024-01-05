using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public class FactionReference : MonoBehaviour
    {
        [HighlightNull] public Faction Faction;
        public bool IsHostileTo(Faction other) => Faction.Hostiles.Contains(other);
        public bool IsAlliedWith(Faction other) => other.Equals(Faction);
    }
}