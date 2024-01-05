namespace Polyperfect.War
{
    public readonly struct AmmoInfo
    {
        public readonly AmmoType AmmoType;
        public readonly int Amount;

        public AmmoInfo(AmmoType ammoType, int amount)
        {
            AmmoType = ammoType;
            Amount = amount;
        }

        public override string ToString() => $"{AmmoType.name}: {Amount}";
    }
}