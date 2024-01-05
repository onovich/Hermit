using System.Linq;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Rider_Carrier))]
    [RequireComponent(typeof(GunAimer))]
    public class InheritAiming : PolyMono
    {
        Rider_Carrier riderLibrary;
        GunAimer aimer;
        public override string __Usage => $"Inherits the aiming function from riders and applies it to the attached {nameof(GunAimer)}.";

        void Awake()
        {
            aimer = GetComponent<GunAimer>();
            riderLibrary = GetComponent<Rider_Carrier>();
            
            riderLibrary.RegisterRiderEnterCallback(HandleRiderEnter);
            riderLibrary.RegisterRiderExitCallback(HandleRiderExit);
        }

        void HandleRiderExit(RiderBase arg0) => SetAimFromAvailable();

        void HandleRiderEnter(RiderBase arg0) => SetAimFromAvailable();

        void SetAimFromAvailable()
        {
            var aim = riderLibrary.Riders.Select(r => r.gameObject.GetComponent<IAimEffector>() as MonoBehaviour).FirstOrDefault(e => e);
            aimer.SetAimingFunction(aim as IAimEffector);
        }
    }
}