using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputCollector))]
    [DefaultExecutionOrder(-60)]
    public class PlayerInputs : PolyMono
    {
        public override string __Usage => $"Inserts player inputs into the attached {nameof(InputCollector)}.";
        InputCollector input;
        public string Horizontal = "Horizontal";
        public string Vertical = "Vertical";
        public string Aim = "Aim";
        public string Shoot = "Shoot";
        public string Reload = "Reload";
        public string Jump = "Jump";
        public string Run = "Run";
        public string WeaponCycleAxis = "Mouse ScrollWheel";
        public string Interact = "Interact";
        Transform relativeTo;

        void Awake()
        {
            input = GetComponent<InputCollector>();

            var mainCam = Camera.main;
            if (mainCam)
                relativeTo = mainCam.transform;
            else
            {
                Debug.LogWarning("No main camera found, defaulting to self");
                relativeTo = transform;
            }
        }

        void Update()
        {
            input[Inputs.Aim].Set(Input.GetButton(Aim));
            input[Inputs.Reload].Set(Input.GetButton(Reload));
            input[Inputs.Shoot].Set(Input.GetButton(Shoot));
            input[Inputs.Jump].Set(Input.GetButton(Jump));
            input[Inputs.Run].Set(Input.GetButton(Run));
            input[Inputs.Interact].Set(Input.GetButton(Interact));
            input[Inputs.Next].Set(Input.GetAxisRaw(WeaponCycleAxis)>0f);
            input[Inputs.Prev].Set(Input.GetAxisRaw(WeaponCycleAxis)<0f);

            var gravity = Physics.gravity.normalized;
            var rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(relativeTo.forward, gravity), -gravity);
            var directTilt = new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical));
            var transformedTilt = Vector3.ClampMagnitude(rot*new Vector3(directTilt.x, 0f,directTilt.y ),1f);
            input[Inputs.Tilt].Set(directTilt,transformedTilt);
            
        }

        
    }
}