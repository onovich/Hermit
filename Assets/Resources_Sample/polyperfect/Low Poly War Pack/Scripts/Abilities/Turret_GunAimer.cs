using System;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class Turret_GunAimer : GunAimer
    {
        public override string __Usage => "Aims the attached gun using traditional turret-style rotation.";
        
        [HighlightNull] [SerializeField] Transform YawObject;
        [SerializeField] Vector3 YawAxis = Vector3.up;
        
        [HighlightNull] [SerializeField] Transform PitchObject;
        [SerializeField] Vector3 PitchAxis = Vector3.right;

        [Range(-89,89)][SerializeField] float MaxAngle = 45f;
        [Range(-89,89)][SerializeField] float MinAngle = -45f;

        [SerializeField] float YawSpeed = 3f;
        [SerializeField] float PitchSpeed = 3f;
        
        AxisTurning yaw;
        ConstrainedAxisTurning pitch;

        void OnValidate()
        {
            MinAngle = Mathf.Min(MinAngle, MaxAngle);
        }

        void Awake()
        {
            yaw = YawObject.gameObject.AddComponent<AxisTurning>();
            yaw.TurnSpeed = YawSpeed;
            yaw.LocalAxis = YawAxis;
            
            pitch = PitchObject.gameObject.AddComponent<ConstrainedAxisTurning>();
            pitch.TurnSpeed = PitchSpeed;
            pitch.LocalAxis = PitchAxis;
            pitch.MaxAngle = MaxAngle;
            pitch.MinAngle = MinAngle;
        }

        protected  void OnEnable()
        {
            yaw.enabled = true;
            pitch.enabled = true;
        }

        protected void OnDisable()
        {
            yaw.enabled = false;
            pitch.enabled = false;
        }

        void Update()
        {
            if (activeAimer==null)
                return;
            
            var aimPos = activeAimer.GetAimPosition();
            var yawPos = YawObject.position;
            var pitchPos = PitchObject.position;
            yaw.TargetDirection = (aimPos - yawPos).normalized;
            pitch.TargetDirection = (aimPos - pitchPos).normalized;
        }

        //public override Vector3 CurrentAimDirection => PitchObject.forward;
        public override Vector3 CurrentAimPosition => PitchObject.TransformPoint(Vector3.forward * Vector3.Distance(PitchObject.position, TargetAimPosition));
    }
}