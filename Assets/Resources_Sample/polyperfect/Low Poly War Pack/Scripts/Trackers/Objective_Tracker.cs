using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(FactionReference))]
    public class Objective_Tracker : Tracker<Objective_Target>
    {
        FactionReference faction;
        public override string __Usage => "Tracks objectives relevant to the unit's faction.";

        void Awake() => faction = GetComponent<FactionReference>();

        protected override bool ShouldTrack(Objective_Target targetable) => targetable.IsObjectiveForFaction(faction.Faction);
    }
}