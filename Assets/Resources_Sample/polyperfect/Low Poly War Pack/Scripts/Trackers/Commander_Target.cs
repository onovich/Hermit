using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(FactionReference))]
    public class Commander_Target : TargetableBase
    {
        public override string __Usage => "Marker for units that can command others.";
        public Faction Faction => factionReference.Faction;
        FactionReference factionReference;
        protected override void Initialize() => factionReference = GetComponent<FactionReference>();
    }
}