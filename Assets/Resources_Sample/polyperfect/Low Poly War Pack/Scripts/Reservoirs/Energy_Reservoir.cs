using UnityEngine;

namespace Polyperfect.War
{
    public class Energy_Reservoir : FloatReservoir
    {
        public override string __Usage => "Holds and regenerates energy";
        public float RegenRate;

        void Update()
        {
            TrySetEnergy(currentAmount + Time.deltaTime * RegenRate);
        }

        void TrySetEnergy(float newAmount)
        {
            ChangeTo(Mathf.Clamp(newAmount, 0f, maxAmount));
        }
    }
}