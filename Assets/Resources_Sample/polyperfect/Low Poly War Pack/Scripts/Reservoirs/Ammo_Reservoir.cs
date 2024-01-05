using UnityEngine;

namespace Polyperfect.War
{
    public class Ammo_Reservoir : Int_Reservoir
    {
        public override string __Usage => $"Holds a particular ammo type for use, typically by a {nameof(Usable)}.";
        [SerializeField] AmmoType ammoType;
        public AmmoType AmmoType => ammoType;
        Ammo_Receiver receiver;
        void Awake()
        {
            receiver = GetComponent<Ammo_Receiver>();
        }

        protected void OnEnable()
        {
            if (!receiver)
                return;
            
            receiver.RegisterInsertConsumerCallback(ReceiveAndOutputRemainder);
            receiver.RegisterExtractConsumerCallback(ExtractAll);
        }

        protected void OnDisable()
        {
            if (!receiver)
                return;
            
            receiver.UnregisterInsertConsumerCallback(ReceiveAndOutputRemainder);
            receiver.UnregisterExtractConsumerCallback(ExtractAll);
        }

        public int ReceiveAndOutputRemainder(AmmoInfo info) => !info.AmmoType.Equals(ammoType) ? info.Amount : InsertPossible(info.Amount);

        public AmmoInfo ExtractAll() => new AmmoInfo(AmmoType,ExtractPossible(int.MaxValue));

        void Reset()
        {
            currentAmount = 0;
            maxAmount = 20;
        }
    }
}