namespace Polyperfect.War
{
    public class AmmoBox_Pickup : Pickup
    {
        public AmmoType AmmoType;
        public int Count = 10;
        public override string __Usage => $"Restores ammo when collected. Automatically tracked by {nameof(SupportedAmmoPack_Tracker)}.";
        protected override void Initialize() { }
    }
}