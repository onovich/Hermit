using UnityEngine;

namespace Polyperfect.War
{
    public class CameraRotationMatch : MonoBehaviour
    {
        Transform target;

        void Start()
        {
            var cam = Camera.main;
            if (!cam)
            {
                Debug.LogError("No main cam found");
                enabled = false;
                return;
            }

            target = cam.transform;
        }

        void LateUpdate() => transform.rotation = target.rotation;
    }
}