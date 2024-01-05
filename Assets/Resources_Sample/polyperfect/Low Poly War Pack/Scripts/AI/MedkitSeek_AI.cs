using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Health_Reservoir))]
    public class MedkitSeek_AI : ResourceSeekerBase_AI<Medkit_Tracker, Medkit_Pickup, Health_Reservoir>
    {
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Movement;
        public override string __Usage => $"Seeks {nameof(Medkit_Pickup)}s in range when {nameof(Health_Reservoir)} drops into or below a certain fraction.";
    }
}