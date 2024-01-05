using System.Collections;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Ammo_Carrier))]
    [RequireComponent(typeof(AmmoReservoir_Proxy))]
    [RequireComponent(typeof(InputCollector))]
    public class Reload_AI : FuzzyComponent
    {
        public override string __Usage =>
            $"Tells the character to reload their {nameof(Usable)} if it has no remaining ammo and they have some to spare in their {nameof(Ammo_Carrier)}";
        Ammo_Carrier ammoLibrary;
        AmmoReservoir_Proxy ammoProxy;
        InputCollector inputs;
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Gunning;

        void Reset()
        {
            WeightMultiplier = 1.5f;
        }

        protected override void Initialize()
        {
            ammoLibrary = GetComponent<Ammo_Carrier>();
            ammoProxy = GetComponent<AmmoReservoir_Proxy>();
            inputs = GetComponent<InputCollector>();
        }

        protected override float CalculateWeight()
        {
            var ammoBeingUsed = ammoProxy.Ammo.Type;
            var needsReload = ammoProxy.Ammo.ActiveRounds <= 0;
            var hasAmmoToReload = ammoBeingUsed&&ammoLibrary.GetRoundCount(ammoBeingUsed)>0;
            return (needsReload && hasAmmoToReload)?1f:0f;
        }

        public override void ActiveUpdate() { }

        IEnumerator ActivateReload()
        {
            yield return new WaitForSeconds(.1f); //this is a janky fix for the current state of the animation controllers requiring not reloading to allow a transition
            inputs[Inputs.Reload].Set(true);
        }
        protected override void HandleActivate()
        {
            StartCoroutine(ActivateReload());
        }

        protected override void HandleDeactivate()
        {
            StopAllCoroutines();
            inputs[Inputs.Reload].Set(false);
        }
    }
}