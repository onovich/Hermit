using System.Collections.Generic;
using UnityEngine;

namespace Polyperfect.War
{
    public class Objective_Target : TargetableBase
    {
        public override string __Usage => "Target for AIs wanting to complete an objective.";
        [SerializeField] List<Faction> ObjectiveForFactions;


        public bool IsObjectiveForFaction(Faction source)
        {
            return ObjectiveForFactions.Contains(source);
        }

        protected override void Initialize()
        {
        }
    }
}