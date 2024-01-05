using UnityEngine;

namespace Polyperfect.War
{
    public abstract class FloatReservoir : Reservoir<float>, IFractionalAmount,IAbsoluteAmount<float>
    {
        public bool ConsumeIfPossible(float amount)
        {
            var newAmount = currentAmount - amount;
            if (newAmount < 0)
                return false;
            currentAmount = newAmount; 
            return true;
        }

        public override float FractionalAmount => currentAmount / maxAmount;

        protected override float doExtractPossible(float amount)
        {
            var nextAmount = currentAmount - amount;
            nextAmount = Mathf.Clamp(nextAmount, 0f, maxAmount);
            var originalAmount = currentAmount;
            ChangeTo(nextAmount);
            return originalAmount - currentAmount;
        }

        public override bool CanFullyExtract(float amount) => currentAmount >= amount;

        protected override float doInsertPossible(float amount)
        {
            var emptySpace = MaxAmount - currentAmount;
            var insertAmount = Mathf.Min(emptySpace, amount);
            ChangeTo(currentAmount + insertAmount);
            return amount - insertAmount;
        }
        
        public override bool CanFullyInsert(float amount) => currentAmount + amount <= maxAmount;
    }
}