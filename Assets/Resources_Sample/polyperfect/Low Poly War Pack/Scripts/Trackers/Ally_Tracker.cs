using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(FactionReference))]
    public class Ally_Tracker : Tracker<Shootable_Target>
    {
        FactionReference faction;
        void Start() => faction = GetComponent<FactionReference>();

        public override string __Usage => "Tracks allies in range.";
        protected override bool ShouldTrack(Shootable_Target targetable) => targetable.AttachedFaction && faction.IsAlliedWith(targetable.AttachedFaction.Faction);
    }
}