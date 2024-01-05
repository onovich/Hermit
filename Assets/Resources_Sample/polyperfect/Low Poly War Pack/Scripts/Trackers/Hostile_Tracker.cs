using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(FactionReference))]
    
    public class Hostile_Tracker : Tracker<Shootable_Target>
    {
        public override string __Usage => $"Tracks entities with a hostile {nameof(Faction)} in range.";
        FactionReference faction;

        protected void Awake()
        {
            faction = GetComponent<FactionReference>();
        }

        protected override bool ShouldTrack(Shootable_Target targetable)
        {
            var healthy = targetable.AttachedHealth && targetable.AttachedHealth.IsAlive;
            var hostile = targetable.AttachedFaction && faction.IsHostileTo(targetable.AttachedFaction.Faction);
            return hostile && healthy;
        }
    }
}