using JetBrains.Annotations;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(FactionReference))]
    public class Faction_Target : TargetableBase
    {
        [CanBeNull] public FactionReference AttachedFaction { get; private set; }
        public override string __Usage => "Tag for anything that might be sought by a Faction";
        protected override void Initialize()
        {
            AttachedFaction = GetComponent<FactionReference>();
        }
    }
}