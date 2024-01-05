using JetBrains.Annotations;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Health_Reservoir))]
    public class Shootable_Target : Faction_Target
    {
        public override string __Usage=>"Tag for anything that an AI may want to shoot.";
        [NotNull] public Health_Reservoir AttachedHealth { get; private set; }
        protected override void Initialize()
        {
            base.Initialize();
            AttachedHealth = GetComponent<Health_Reservoir>();
        }
    }
}