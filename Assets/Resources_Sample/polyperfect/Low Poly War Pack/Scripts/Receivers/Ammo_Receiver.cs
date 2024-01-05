using System;
using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public class Ammo_Receiver : PolyMono
    {
        public override string __Usage => "Allows receiving of ammo. Does not do anything with the ammo by itself.";
        readonly List<Func<AmmoInfo, int>> insertConsumers = new List<Func<AmmoInfo, int>>();
        readonly List<Func<AmmoInfo>> extractConsumers = new List<Func<AmmoInfo>>();

        public void RegisterInsertConsumerCallback(Func<AmmoInfo, int> receiveAndOutputRemainder) => insertConsumers.Add(receiveAndOutputRemainder);
        public void UnregisterInsertConsumerCallback(Func<AmmoInfo, int> receiveAndOutputRemainder) => insertConsumers.Remove(receiveAndOutputRemainder);

        public void RegisterExtractConsumerCallback(Func<AmmoInfo> extractAllAmmo) => extractConsumers.Add(extractAllAmmo);
        public void UnregisterExtractConsumerCallback(Func<AmmoInfo> extractAllAmmo) => extractConsumers.Remove(extractAllAmmo);
        public AmmoInfo InsertPossible(AmmoInfo ammo)
        {
            foreach (var item in insertConsumers) 
                ammo = new AmmoInfo(ammo.AmmoType, item(ammo));

            return ammo;
        }

        public List<AmmoInfo> ExtractAllAmmo() => extractConsumers.Select(item => item()).ToList();
    }
}