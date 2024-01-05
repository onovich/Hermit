using JetBrains.Annotations;

namespace Polyperfect.War
{
    public readonly struct ActiveAmmoInfo
    {
        [CanBeNull] public readonly AmmoType Type;
        public readonly int ActiveRounds;
        public readonly int ClipSize;

        public ActiveAmmoInfo(AmmoType type, int activeRounds, int clipSize)
        {
            Type = type;
            ActiveRounds = activeRounds;
            ClipSize = clipSize;
        }
    }
}