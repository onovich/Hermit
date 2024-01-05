using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(GunAimer))]
    public class MouseAndCamera_AimEffector : PolyMono,IAimEffector
    {
        public override string __Usage => "Aims using the mouse and camera. The cursor position is considered the center of the screen if locked.";
        Camera cam;
        GunAimer aiming;
        public LayerMask HitMask = int.MaxValue;
        void Awake()
        {
            aiming = GetComponent<GunAimer>();
            cam = Camera.main;
        }

        void OnEnable()
        {
            aiming.SetAimingFunction(this);
        }

        public Vector3 GetAimPosition()
        {
            if (Physics.Raycast(cam.ScreenPointToRay(CursorLocker.CursorPosition()), out var hit, Mathf.Infinity, HitMask))
                return hit.point;
            else
                return cam.transform.TransformPoint(new Vector3(0f, 0f, 30f));
        }
    }
}