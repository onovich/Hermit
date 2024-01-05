using System;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Polyperfect.War
{
    [RequireComponent(typeof(InputCollector))]
    public class TiltTowardsPosition_Input : PolyMono
    {
        public override string __Usage =>
            "Generates inputs that tilt towards a particular position. If a NavMesh is present, it will use that instead of directly tilting";

        InputCollector input;
        public float StopDistance = 0.3f;
        public Vector3 TargetPosition { get; protected set; }
        public bool IsSeeking { get; protected set; }
        float tiltMagnitude;

        [SerializeField] UnityEvent OnArrivedAtTarget;
        NavMeshAgent navAgent;

        void Awake()
        {
            input = GetComponent<InputCollector>();
            OnArrivedAtTarget = OnArrivedAtTarget ?? new UnityEvent();
            path = new NavMeshPath();
        }

        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            if (navAgent && (navAgent.updatePosition || navAgent.updateRotation))
                Debug.LogWarning(
                    $"The {nameof(NavMeshAgent)} on {gameObject.name} has {nameof(NavMeshAgent.updatePosition)} or {nameof(NavMeshAgent.updateRotation)} enabled, which may cause undesired behavior.");
        }

        protected void Update()
        {
            Vector3 targPos;
            if (isUsingPathing&& DoPath(TargetPosition))
            {
                navAgent.nextPosition = transform.position;
                targPos = navAgent.steeringTarget+transform.forward*.2f; 
            }
            else
            {
                targPos = TargetPosition;
            }

            var projected = GetFlattenedDisplacement(targPos);
            var inputWorldTiltVector = Vector3.ClampMagnitude(projected * 2F, tiltMagnitude);
            var directVector = transform.InverseTransformDirection(inputWorldTiltVector);
            input[Inputs.Tilt].Set(new Vector2(directVector.x, directVector.z), inputWorldTiltVector);
            Debug.DrawLine(transform.position, targPos);
            if (projected.magnitude <= StopDistance)
            {
                Arrived();
            }
        }

        void Arrived()
        {
            IsSeeking = false;
            var del = currentNavigationEnd;
            currentNavigationEnd = () => { };
            del();
            OnArrivedAtTarget.Invoke();
        }

        Vector3 GetFlattenedDisplacement(Vector3 target)
        {
            var displacement = target - transform.position;
            var projected = Vector3.ProjectOnPlane(displacement, Vector3.up);
            projected = Vector3.MoveTowards(projected, Vector3.zero, StopDistance);
            return projected;
        }

        Action currentNavigationEnd = () => { };
        NavMeshPath path;
        bool isUsingPathing => navAgent&&navAgent.isOnNavMesh;

        public void SetTarget(Vector3 position, float tiltAmount = 1f, Action onNavigationEnd = null)
        {
            tiltMagnitude = tiltAmount;
            if (IsSeeking)
                Arrived();

            currentNavigationEnd = onNavigationEnd ?? (() => { });

            if (isUsingPathing)
            {
                 DoPath(position);
            }

            TargetPosition = position;
            IsSeeking = true;
        }

        bool DoPath(Vector3 position)
        {
            if (!navAgent.CalculatePath(position, path))
                return false;
            navAgent.path = path;
            return true;
        }

        public void RegisterReachedDestinationCallback(UnityAction action)
        {
            OnArrivedAtTarget.AddListener(action);
        }
    }
}