using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(FactionReference))]
    public class Commander_Tracker : Tracker<Commander_Target>
    {
        public override string __Usage => $"Tracks nearby {nameof(Commander_Target)}s.";
        
        FactionReference faction;
        protected void Awake() => faction = GetComponent<FactionReference>();

        protected override bool ShouldTrack(Commander_Target targetable)
        {
            if (!targetable.Faction)
                return false;
            return faction.IsAlliedWith(targetable.Faction);
        }

    }
}