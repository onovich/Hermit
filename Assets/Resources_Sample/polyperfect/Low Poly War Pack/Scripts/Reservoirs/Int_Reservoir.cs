using System;
using UnityEngine;

namespace Polyperfect.War
{
    public abstract class Int_Reservoir : Reservoir<int>
    {
        public bool TryUse(int amount)
        {
            var newAmount = currentAmount - amount;
            if (newAmount < 0)
                return false;
            currentAmount = newAmount; 
            return true;
        }

        public override float FractionalAmount => currentAmount / (float)maxAmount;
        
        
        protected override int doExtractPossible(int amount)
        {
            var nextAmount = currentAmount - amount;
            nextAmount = Mathf.Clamp(nextAmount, 0, maxAmount);
            var originalAmount = currentAmount;
           ChangeTo(nextAmount);
            return originalAmount - currentAmount;
        }

        public override bool CanFullyExtract(int amount) => currentAmount >= amount;

        public bool IsAtMax() => currentAmount >= maxAmount;

        protected override int doInsertPossible(int amount)
        {
            var emptySpace = MaxAmount - currentAmount;
            var insertAmount = Mathf.Min(emptySpace, amount);
            ChangeTo(currentAmount + insertAmount);
            return amount - insertAmount;
        }

        public override bool CanFullyInsert(int amount) => currentAmount + amount <= maxAmount;
    }
}