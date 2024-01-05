using System;
using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public class Rider_Carrier : PolyMono,IRideable
    {
        public override string __Usage => $"Holds riders of the vehicle. Handles calling {nameof(RiderBase.EnterRide)} and {nameof(RiderBase.ExitRide)} on riders. If an {nameof(InputCollector)} is attached, it will reset it to default values on the rider exiting.";
        readonly Dictionary<RiderBase, RideEnterContext> riderContexts = new Dictionary<RiderBase, RideEnterContext>();
        [SerializeField] int MaxRiders = 1;
        [SerializeField] UnityEvent<RiderBase> OnRiderEnter, OnRiderExit;
        public IEnumerable<RiderBase> Riders => riderContexts.Keys;
        public bool DisableColliders;
        public bool DisableRenderers;
        bool applicationIsRunning = true;

        void Awake()
        {
            if (gameObject.TryGetComponent(out InputCollector collector))
            {
                RegisterRiderExitCallback(e=>collector.ResetToDefaults());
            }
        }
#if UNITY_EDITOR

        void Start()
        {
            //check to keep events from being called and potentially spawning objects when destroying
            UnityEditor.EditorApplication.playModeStateChanged += p =>
            {
                if (p == UnityEditor.PlayModeStateChange.ExitingPlayMode)   
                    applicationIsRunning = false;
            };
        }
#endif
        public void AddRider(RiderBase rider)
        {
            var context = new RideEnterContext(this,DisableColliders,DisableRenderers);
            riderContexts.Add(rider,context);
            rider.EnterRide(context);
            OnRiderEnter.Invoke(rider);
        }

        public void RemoveRider(RiderBase rider, Vector3 position, Quaternion rotation)
        {
            var enterContext = riderContexts[rider];
            var exitContext = new RideExitContext(enterContext.Rideable, position, rotation);
            riderContexts.Remove(rider);
            rider.ExitRide(exitContext);
            OnRiderExit.Invoke(rider);
        }

        void OnDisable()
        {
            if (applicationIsRunning)
                EjectAll();
        }

        public void RegisterRiderEnterCallback(UnityAction<RiderBase> act) => OnRiderEnter.AddListener(act);
        public void RegisterRiderExitCallback(UnityAction<RiderBase> act) => OnRiderExit.AddListener(act);
        
        public void UnregisterRiderEnterCallback(UnityAction<RiderBase> act) => OnRiderEnter.RemoveListener(act);
        public void UnregisterRiderExitCallback(UnityAction<RiderBase> act) => OnRiderExit.RemoveListener(act);

        public bool HasRider(RiderBase contextUser) => riderContexts.ContainsKey(contextUser);
        public bool SupportsRider(RiderBase prospectiveRider)
        {
            var supportsRider = riderContexts.Count < MaxRiders;
            return supportsRider;
        }

        public void EjectAll()
        {
            foreach (var rider in Riders.ToList())
            {
                RemoveRider(rider,transform.position,transform.rotation);
            }
        }

        public Transform GetSlot() => transform;
    }
}