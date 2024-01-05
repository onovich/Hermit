namespace Polyperfect.War
{
    public class AmmoSeek_AI : SeekerBase_AI<SupportedAmmoPack_Tracker,AmmoBox_Pickup>
    {
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Movement;
        public override string __Usage => $"Seeks {nameof(AmmoBox_Pickup)}s in range. Only goes after those not at max capacity with a relevant weapon.";
    }
}