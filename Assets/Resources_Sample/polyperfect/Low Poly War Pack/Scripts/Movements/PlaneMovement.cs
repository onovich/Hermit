using System.Collections;
using System.Collections.Generic;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [System.Serializable]
    public class Wheel
    {
        public WheelCollider wheelCollider;
        public bool steering;
        public bool brake;
    }

    [System.Serializable]
    public class Wing
    {
        public float areaSurface;
        public Transform wingTransform;
    }

    [RequireComponent(typeof(InputCollector))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class PlaneMovement : PolyMono
    {
        public override string __Usage => "Plane.";
        public VirtualCamera virtualCamera;
        public float LiftThreshold = 17f;

        public float currentSpeed = 0f;
        [Range(100, 1000)] public float maxSpeed = 500f;
        [Range(100, 5000)] public float brakePower;
        [Range(0, 90)] public float maxSteeringAngle;
        [Range(100, 100000)] public float engineTorque;
        [Range(0, 1000)] public float trailEmittingSpeed;

        public Wing planeVertical;
        public Wing planeHorizontal;
        public Wing rightWing;
        public Wing leftWing;
        public Wing rightHorizontalStabilizer;
        public Wing leftHorizontalStabilizer;
        public Wing verticalStabilizer;
        public Wing rudder;
        public Wing rightElevator;
        public Wing leftElevator;
        public Wing rightAileron;
        public Wing leftAileron;

        public bool backTurningWheel;
        public List<Wheel> wheels = new List<Wheel>();
        public List<TrailRenderer> trails = new List<TrailRenderer>();

        public AerodynamicCurves aerodynamicCurves;
        public AnimationCurve steeringCurve;


        //Audio Parameters
        float min = 5f;
        float max = -5f;
        float newMin = 0.8f;
        float newMax = 1.3f;

        //Input Parameters
        private float vertical;
        private float horizontal;
        private float engineThrottle = 0;

        public Transform[] propellers;
        public Vector3 centerOfMass;

        public Vector2 rollRange;
        public Vector2 yawRange;
        public Vector2 pitchRange;

        private Animator animator;
        private Rigidbody rb;
        private AudioSource planeFlying;
        private Transform cam;
        private Vector3 targetAim;

        float roll;
        float yaw;
        float pitch;
        float audioPitch;


        bool wheelsUp = false;
        bool emiting = false;
        InputCollector input;
        bool interactingWith;

        void Awake()
        {
            input = GetComponent<InputCollector>();
            if (gameObject.TryGetComponent(out Rider_Carrier riderLibrary))
            {
                riderLibrary.RegisterRiderEnterCallback(HandleEnter);
                riderLibrary.RegisterRiderExitCallback(HandleExit);
            }

            input[Inputs.Reload].OnActivate += HandleLandingGear;
        }

        void HandleExit(RiderBase arg0)
        {
            interactingWith = false;
            planeFlying.Stop();
        }

        void HandleEnter(RiderBase arg0)
        {
            interactingWith = true;
            planeFlying.Play();
        }

        void Start()
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = false;
            }

            cam = Camera.main.transform;
            planeFlying = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            rb.centerOfMass = centerOfMass;

            foreach (Wheel wheel in wheels)
            {
                if (wheel.brake)
                    wheel.wheelCollider.brakeTorque = brakePower;
            }
        }

        private void Update()
        {
            if (interactingWith)
            {
                var localInput = input[Inputs.Tilt].DirectVector;//LocalTiltVector;
                vertical = localInput.y;//Input.GetAxis("Vertical");
                horizontal = localInput.x;//Input.GetAxis("Horizontal");

                engineThrottle += vertical;
                engineThrottle = Mathf.Clamp(engineThrottle, 0, 100);
                foreach (Transform propeller in propellers)
                    propeller.localRotation = Quaternion.Euler(propeller.localRotation.eulerAngles.x, 0, propeller.localRotation.eulerAngles.z + engineThrottle);
                

                if (rb.velocity.magnitude > trailEmittingSpeed)
                {
                    if (!emiting)
                    {
                        emiting = true;
                        foreach (TrailRenderer trail in trails)
                        {
                            trail.emitting = true;
                        }
                    }
                }
                else if (emiting)
                {
                    emiting = false;
                    foreach (TrailRenderer trail in trails)
                    {
                        trail.emitting = false;
                    }
                }
            }
        }

        void HandleLandingGear()
        {
            if (!animator)
                return;
            if (animator.GetBool("WheelsUp"))
            {
                StartCoroutine(WaitForWheelsDown());
                animator.SetBool("WheelsUp", false);
            }
            else
            {
                wheelsUp = true;
                animator.SetBool("WheelsUp", true);
            }
        }

        private void FixedUpdate()
        {
            if (interactingWith)
            {
                float force = engineThrottle * 0.01f * engineTorque;
                if (currentSpeed < maxSpeed)
                {
                    foreach (Transform propeller in propellers)
                        rb.AddForceAtPosition(propeller.forward * force, propeller.position, ForceMode.Force);
                }

                var localInput = input[Inputs.Tilt].DirectVector;
                float brake = brakePower * localInput.y;//Input.GetAxis("Vertical");
                float steering = maxSteeringAngle * localInput.x;//Input.GetAxis("Horizontal");
                float speed = (rb.velocity.magnitude);
                currentSpeed = speed;
                /*    RaycastHit hit;
                    if (Physics.Raycast(cam.position, cam.forward, out hit, 1000))
                    {
                        targetAim = hit.point;
                    }
                    else*/
                targetAim = cam.position + cam.forward * 1000;
                Vector3 direction = targetAim - virtualCamera.followTarget.position;
                float deviation = Vector3.Angle(direction, virtualCamera.followTarget.forward) / 90;
                deviation = Mathf.Clamp(deviation, 0, 1);
                direction = direction.normalized;
                roll = transform.right.y * 10;
                yaw = Vector3.SignedAngle(virtualCamera.followTarget.forward, Vector3.ProjectOnPlane(direction, virtualCamera.followTarget.up),
                    -virtualCamera.followTarget.up);
                pitch = Vector3.SignedAngle(virtualCamera.followTarget.forward, Vector3.ProjectOnPlane(direction, virtualCamera.followTarget.right),
                    -virtualCamera.followTarget.right);
                roll = Mathf.Lerp(roll, -yaw, deviation);

                roll = Mathf.Clamp(roll, rollRange.x, rollRange.y);
                yaw = Mathf.Clamp(yaw, yawRange.x, yawRange.y);
                pitch = Mathf.Clamp(pitch, pitchRange.x, pitchRange.y);
                if (input[Inputs.Aim].Value)
                {
                    pitch = 0;
                    yaw = 0;
                    roll = 0;
                }

                rightAileron.wingTransform.localRotation = Quaternion.Euler(roll, 0, 0);
                leftAileron.wingTransform.localRotation = Quaternion.Euler(-roll, 0, 0);
                rudder.wingTransform.localRotation = Quaternion.Euler(0, yaw, -90);
                rightElevator.wingTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
                leftElevator.wingTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
                
                if (speed > LiftThreshold)
                {
                    CalculateAerodynamicForce(rightWing);
                    CalculateAerodynamicForce(leftWing);
                    CalculateAerodynamicForce(leftHorizontalStabilizer);
                    CalculateAerodynamicForce(rightHorizontalStabilizer);
                    CalculateAerodynamicForce(rightElevator);
                    CalculateAerodynamicForce(rudder);
                    CalculateAerodynamicForce(leftElevator);
                    CalculateAerodynamicForce(planeHorizontal);
                    CalculateAerodynamicForce(planeVertical);
                    CalculateAerodynamicForce(verticalStabilizer);
                    CalculateAerodynamicForce(rightAileron);
                    CalculateAerodynamicForce(leftAileron);
                }

                if (speed > 25 && brake < 0)
                    rb.AddForce(rb.velocity.normalized * brake * 100 * (rb.velocity.magnitude / maxSpeed), ForceMode.Force);
                Sound();
                steering *= steeringCurve.Evaluate(speed / maxSpeed);
                foreach (Wheel wheel in wheels)
                {
                    if (wheel.brake)
                    {
                        if (wheel.wheelCollider.isGrounded)
                        {
                            if (speed > 0 && brake < 0)
                            {
                                brake = Mathf.Abs(brake);
                                wheel.wheelCollider.brakeTorque = brake;
                            }
                            else
                                wheel.wheelCollider.brakeTorque = 0;

                            if (force > 0)
                                wheel.wheelCollider.motorTorque = 10;
                            else
                                wheel.wheelCollider.motorTorque = 0;
                        }
                    }

                    if (wheel.steering)
                    {
                        if (backTurningWheel)
                            wheel.wheelCollider.steerAngle = -steering;
                        else
                            wheel.wheelCollider.steerAngle = steering;
                    }

                    if (!wheelsUp)
                        ApplyLocalPositionToVisuals(wheel.wheelCollider);
                }

                //LookAround();
            }
            else
            {
                if (!wheelsUp)
                {
                    foreach (Wheel wheel in wheels)
                    {
                        ApplyLocalPositionToVisuals(wheel.wheelCollider);
                    }
                }
            }
        }


        void CalculateAerodynamicForce(Wing wing)
        {
            Vector3 localVelocity = wing.wingTransform.InverseTransformDirection(rb.GetPointVelocity(wing.wingTransform.position));
            localVelocity.x = 0f;
            float angle = Vector3.Angle(Vector3.forward, localVelocity);
            rb.AddForceAtPosition(
                -Mathf.Sign(localVelocity.y) * Vector3.Cross(rb.velocity, wing.wingTransform.right).normalized * 0.6125f * localVelocity.sqrMagnitude * wing.areaSurface *
                aerodynamicCurves.liftCurve.Evaluate(angle), wing.wingTransform.position, ForceMode.Force);
            rb.AddForceAtPosition(-rb.velocity.normalized * 0.6125f * localVelocity.sqrMagnitude * wing.areaSurface * aerodynamicCurves.dragCurve.Evaluate(angle),
                wing.wingTransform.position, ForceMode.Force);
        }

        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount == 0)
            {
                return;
            }

            Transform visualWheel = collider.transform.GetChild(0);

            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);

            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }

        void Sound()
        {
            if (interactingWith)
            {
                audioPitch = Mathf.Abs(horizontal) - vertical;

                planeFlying.pitch = audioPitch.Remap(min, max, newMin, newMax);
            }
        }

        bool GroundCheck(float height)
        {
            if (Physics.Raycast((transform.position + transform.forward * 2), Vector3.down, out RaycastHit hit, height))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator WaitForWheelsDown()
        {
            yield return new WaitForSeconds(1);
            wheelsUp = false;
        }
    }
}