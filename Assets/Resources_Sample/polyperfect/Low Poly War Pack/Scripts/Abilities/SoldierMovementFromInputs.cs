using System;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InputCollector))]
    [RequireComponent(typeof(CharacterController))]
    public class SoldierMovementFromInputs : PolyMono
    {
        public override string __Usage =>
            $"Uses the values in the attached {nameof(InputCollector)} to move the character.\nAutomatically deactivates on death if there's an attached {nameof(Health_Reservoir)}.\nAutomatically disabled when riding a vehicle via {nameof(RiderBase)}.";

        [Range(0.1f, 10f)] public float WalkSpeed = 5;

        [Range(1f, 15f)] public float RunSpeed = 12;
        [Range(.1f, 60f)] public float TurnSpeed = 12;
        [Range(0f, 10f)] public float JumpSpeed = 5;
        [Range(0f, 20f)] public float Acceleration = 10f;
        [Range(0f, 1f)] public float TurnThreshold = .15f;
        [SerializeField] UnityEvent OnJump;
        bool isGrounded => controller.isGrounded;
        float StickStrength = 1f;
        public bool IsRunning => input[Inputs.Tilt].DirectVector.sqrMagnitude>.1f&&input[Inputs.Run].Value&&!input[Inputs.Aim].Value;
        public Vector3 Velocity { get; protected set; }
        public Vector3 LocalVelocity => transform.InverseTransformDirection(Velocity);

        InputCollector input;
        CharacterController controller;
        AxisTurning turning;

        void Awake()
        {
            input = GetComponent<InputCollector>();
            controller = GetComponent<CharacterController>();
            turning = gameObject.AddComponent<AxisTurning>();
            turning.TurnSpeed = TurnSpeed;

            HandleOptionalComponents();
        }

        void HandleOptionalComponents()
        {
            if (gameObject.TryGetComponent(out GunAimer aiming))
            {
                aiming.RegisterBeginAimingCallback(() => turning.enabled = false);
                aiming.RegisterEndAimingCallback(() =>
                {
                    turning.TargetDirection = transform.forward;
                    turning.enabled = true;
                });
            }

            if (gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                navMeshAgent.updatePosition = false;
                navMeshAgent.updateRotation = false;
                navMeshAgent.autoBraking = false;
            }

            if (gameObject.TryGetComponent(out Health_Reservoir health))
            {
                health.RegisterDeathCallback(() => enabled = false);
                health.RegisterReviveCallback(() => enabled = true);
            }

            if (gameObject.TryGetComponent(out RiderBase riderBase))
            {
                riderBase.RegisterEnterCallback(c => enabled = false);
                riderBase.RegisterExitCallback(c => enabled = true);
            }
        }

        void Update()
        {
            if (!controller.enabled)
                return;
            
            var gravity = Physics.gravity;
            var vertComponent = new Vector3(0f, Velocity.y, 0f);
            var horizComponent = new Vector3(Velocity.x, 0, Velocity.z);
            var targetSpeed = (IsRunning&&!input[Inputs.Aim].Value) ? RunSpeed : WalkSpeed;
            vertComponent += gravity * Time.deltaTime;

            if (isGrounded)
                vertComponent = gravity.normalized * StickStrength;
            if (input[Inputs.Jump].RecentlyActivated && controller.isGrounded)
            {
                vertComponent += Vector3.up * JumpSpeed;
                OnJump.Invoke();
            }

            var projTilt = Vector3.ProjectOnPlane(input[Inputs.Tilt].WorldVector, gravity.normalized);
            horizComponent = Vector3.MoveTowards(horizComponent, projTilt * targetSpeed, Acceleration * Time.deltaTime);

            Velocity = vertComponent + horizComponent;

            controller.Move(Velocity * Time.deltaTime);

            if (input[Inputs.Tilt].WorldVector.sqrMagnitude > TurnThreshold * TurnThreshold)
                turning.TargetDirection = horizComponent;
        }

        void OnDisable()
        {
            Velocity = Vector3.zero;
        }

        public void RegisterJumpCallback(UnityAction action) => OnJump.AddListener(action);
        public void UnregisterJumpCallback(UnityAction act) => OnJump.RemoveListener(act);
    }
}