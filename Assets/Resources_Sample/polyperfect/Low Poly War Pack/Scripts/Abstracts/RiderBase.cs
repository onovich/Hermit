using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [RequireComponent(typeof(InputCollector))]
    public abstract class RiderBase : PolyMono
    {
        [HighlightNull] [SerializeField] UsableCapability EnterCapability;
        [HighlightNull] [SerializeField] UsableCapability ExitCapability;
        [SerializeField] Transform CheckLocation;
        [SerializeField] float CheckRadius = 2f;
        [LowPriority][SerializeField] UnityEvent<RideEnterContext> OnEnter;
        [LowPriority][SerializeField] UnityEvent<RideExitContext> OnExit;

        protected InputCollector input;

        readonly List<Collider> colliders = new List<Collider>();
        readonly List<Renderer> renderers = new List<Renderer>();
        IRideable rideable;
        Transform checkTransform => CheckLocation ? CheckLocation : transform;
        bool isRiding => rideable?.gameObject;
        static Collider[] overlaps = new Collider[100];

        public bool TestEveryFrame = false;
        bool inRangeLastFrame;
        public UnityEvent OnEnterRange, OnExitRange;
        protected void Awake()
        {
            input = GetComponent<InputCollector>();
        }

        void OnEnable()
        {
            input[Inputs.Interact].OnActivate += HandleInteract;
        }

        void OnDisable()
        {
            input[Inputs.Interact].OnActivate -=HandleInteract;
        }

        void HandleInteract()
        {
            if (!isRiding)
            {
                var count = DoCheck();
                if (count > 0)
                {
                    var context = new UseContext(EnterCapability, gameObject);
                    for (var i = 0; i < count; i++)
                    {
                        var col = overlaps[i];
                        var usable = col.GetComponentInParent<Usable>();
                        if (usable && usable.TryUse(context, out _))
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                var usable = rideable.gameObject.GetComponent<Usable>();
                if (usable)
                    usable.TryUse(new UseContext(ExitCapability, gameObject), out _);
            }
        }

        void Update()
        {
            if (!TestEveryFrame)
                return;
            
            var wasAbleToEnter = false;
            if (!isRiding)
            {
                var count = DoCheck();
                if (count > 0)
                {
                    var context = new UseContext(EnterCapability, gameObject);
                    for (var i = 0; i < count; i++)
                    {
                        var col = overlaps[i];
                        var usable = col.GetComponentInParent<Usable>();
                        if (usable&&usable.CanUse(context))
                        {
                            wasAbleToEnter = true;
                            break;
                            
                        }
                    }
                }
            }


            switch (wasAbleToEnter)
            {
                case true when !inRangeLastFrame:
                    OnEnterRange.Invoke();
                    break;
                case false when inRangeLastFrame:
                    OnExitRange.Invoke();
                    break;
            }

            inRangeLastFrame = wasAbleToEnter;
        }

        int DoCheck()
        {
            return Physics.OverlapSphereNonAlloc(checkTransform.position, CheckRadius, overlaps);
        }

        public void EnterRide(RideEnterContext context)
        {
            rideable = context.Rideable;
            if (context.DisableColliders)
                foreach (var item in GetComponentsInChildren<Collider>())
                {
                    if (item.enabled)
                    {
                        colliders.Add(item);
                        item.enabled = false;
                    }
                }

            if (context.DisableRenderers)
                foreach (var rend in GetComponentsInChildren<Renderer>())
                {
                    if (rend.enabled)
                    {
                        renderers.Add(rend);
                        rend.enabled = false;
                    }
                }

            HandleEnterRideable(context);
            OnEnter.Invoke(context);
        }

        public void ExitRide(RideExitContext context)
        {
            HandleExitRideable(context);
            rideable = null;

            foreach (var item in colliders.Where(c=>c)) 
                item.enabled = true;
            foreach (var item in renderers.Where(r=>r)) 
                item.enabled = true;

            colliders.Clear();
            renderers.Clear();
            OnExit.Invoke(context);
        }

        protected abstract void HandleEnterRideable(RideEnterContext context);
        protected abstract void HandleExitRideable(RideExitContext context);



        public void RegisterEnterCallback(UnityAction<RideEnterContext> action) => OnEnter.AddListener(action);
        public void RegisterExitCallback(UnityAction<RideExitContext> action) => OnExit.AddListener(action);
        
        public void UnregisterEnterCallback(UnityAction<RideEnterContext> action) => OnEnter.RemoveListener(action);
        public void UnregisterExitCallback(UnityAction<RideExitContext> action) => OnExit.RemoveListener(action);
    }
}