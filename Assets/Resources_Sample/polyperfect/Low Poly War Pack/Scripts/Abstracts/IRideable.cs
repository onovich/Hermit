using UnityEngine;

namespace Polyperfect.War
{
    public interface IRideable
    {
        Transform GetSlot();
        GameObject gameObject { get; }

        void AddRider(RiderBase rider);
        //Usable Usable { get; }
    }
}