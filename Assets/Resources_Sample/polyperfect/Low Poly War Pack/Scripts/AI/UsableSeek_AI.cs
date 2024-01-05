using System;

namespace Polyperfect.War
{
    public class UsableSeek_AI : SeekerBase_AI<Supported_UsablePickup_Tracker, Usable_Pickup>
    {
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Movement;
        public override string __Usage => $"Makes the character seek {nameof(Usable)}s it can wield.";

        void Reset() => WeightMultiplier = .4f;
    }
}