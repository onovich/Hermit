using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(RectTransform))]
    
    public class ScreenSpaceReticle : PolyMono
    {
        RectTransform rectTransform;
        Camera _camera;
        GunAimer aimer;
        public Transform AimPositionOverride;
        public override string __Usage => $"Makes a UI element move along with an aiming target. Uses {nameof(AimPositionOverride)} or otherwise the attached {nameof(GunAimer)}";
        public float SmoothTime = .05f;
        public float CenterSnapLeniency = .02f;

        void Awake()
        {
            _camera = Camera.main;
            rectTransform = GetComponent<RectTransform>();
            if (AimPositionOverride)
                return;
            
            aimer = GetComponentInParent<GunAimer>();
            if (!aimer)
            {
                Debug.LogError($"Either an {nameof(AimPositionOverride)} must be assigned or a parent of {gameObject.name} must have an {nameof(GunAimer)} attached to work with {nameof(ScreenSpaceReticle)}.");
                enabled = false;
            }
            
        }

        Vector3 lastWorldAimPos;
        Vector3 posVel;
        void FixedUpdate()
        {
            var screenAimPosition = (Vector2)_camera.WorldToScreenPoint(AimPositionOverride?AimPositionOverride.position:aimer.CurrentAimPosition);
            var disparity = Vector3.Distance(_camera.ScreenToViewportPoint(screenAimPosition), new Vector2(.5f, .5f));
            if (disparity < CenterSnapLeniency)
                screenAimPosition = new Vector2(Screen.width * .5f, Screen.height * .5f);
            screenAimPosition = Vector3.SmoothDamp(lastWorldAimPos, screenAimPosition, ref posVel, SmoothTime);
            rectTransform.position = screenAimPosition;
            lastWorldAimPos = screenAimPosition;
        }
    }
}