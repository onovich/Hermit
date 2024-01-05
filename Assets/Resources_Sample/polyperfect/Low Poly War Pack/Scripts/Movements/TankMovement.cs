using System.Collections.Generic;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [System.Serializable]
    public class TankWheel
    {
        public WheelCollider wheelCollider;
        public Transform beltBone;
    }

    [System.Serializable]
    public class CosmeticWheel
    {
        public Transform wheel;
        public Transform beltBone;
    }
    [RequireComponent(typeof(Rigidbody), typeof(AudioSource), typeof(UnityEngine.AI.NavMeshObstacle))]
    [RequireComponent(typeof(InputCollector))]
    [RequireComponent(typeof(Rider_Carrier))]
    public class TankMovement : PolyMono
    {
        public override string __Usage => "Tank.";

        [Range(100f, 10000f)] public float motorTorque = 1000;
        [Range(100f, 10000f)] public float brakePower = 1000;
        [Range(10f, 80f)] public float maxSpeed = 40;
        [Range(5f, 90f)] public float turnSpeed = 20;
        public SkinnedMeshRenderer rightBeltRenderer;
        public SkinnedMeshRenderer leftBeltRenderer;
        public Vector3 centerOfMass;
        public TankWheel[] rightBeltWheels;
        public TankWheel[] leftBeltWheels;
        public List<CosmeticWheel> cosmeticWheels;



        [System.Serializable]
        public class AudioParams
        {
            //Audio Parameters
            public float min = 1f;
            public float max = -1f;
            public float newMin = 0.8f;
            public float newMax = 1.3f;
        }

        public AudioParams audioParams;

        //Input Parameters
        float vertical;
        float horizontal;

        bool startedEngine = false;
        bool IsInteracting;
        RiderBase rider;
        private AudioSource tankMovingSource;
        private Vector3 lastPos;
        private Rigidbody rb;
        InputCollector input;
            
            
        bool canGetOut => true;

        void Awake()
        {
            input = GetComponent<InputCollector>();
            var riderLibrary = GetComponent<Rider_Carrier>();
            riderLibrary.RegisterRiderEnterCallback(r => IsInteracting = true);
            riderLibrary.RegisterRiderExitCallback(r=>IsInteracting=false);
            riderLibrary.RegisterRiderExitCallback(r=>GetOutOfTank());
        }

        // Use this for initialization
        void Start()
        {
            lastPos = transform.position;
            rb = GetComponent<Rigidbody>();
            tankMovingSource = GetComponent<AudioSource>();
            rb.centerOfMass = centerOfMass;
        }

        private void Update()
        {
            if (IsInteracting)
            {
                Sound();
            }
        }

        void FixedUpdate()
        {
            //if (IsInteracting)
            {
                var localInputVector = input[Inputs.Tilt].DirectVector;
                vertical = -localInputVector.y; 
                horizontal = localInputVector.x;

                float motor = motorTorque * localInputVector.y; 

                float brake = brakePower * localInputVector.y; 
                float speed = ((transform.position - lastPos).magnitude / Time.fixedDeltaTime);
                float vectorSpeed = speed;
                if (Vector3.Angle(transform.forward, (transform.position - lastPos)) > 90)
                    vectorSpeed = -vectorSpeed;
                if (rightBeltWheels[3].wheelCollider.rpm > 0)
                    rightBeltRenderer.material.mainTextureOffset = new Vector2(0,
                        rightBeltRenderer.sharedMaterial.mainTextureOffset.y + (speed / maxSpeed * Time.fixedDeltaTime));
                else
                    rightBeltRenderer.material.mainTextureOffset = new Vector2(0,
                        rightBeltRenderer.sharedMaterial.mainTextureOffset.y - (speed / maxSpeed * Time.fixedDeltaTime));
                if (leftBeltWheels[3].wheelCollider.rpm > 0)
                    leftBeltRenderer.material.mainTextureOffset = new Vector2(0,
                        rightBeltRenderer.sharedMaterial.mainTextureOffset.y + (speed / maxSpeed * Time.fixedDeltaTime));
                else
                    leftBeltRenderer.material.mainTextureOffset = new Vector2(0,
                        rightBeltRenderer.sharedMaterial.mainTextureOffset.y - (speed / maxSpeed * Time.fixedDeltaTime));
                lastPos = transform.position;

                float turn = localInputVector.x; 
                if (motor == 0)
                    transform.Rotate(Vector3.up, turn * turnSpeed * Time.fixedDeltaTime);
                else
                    transform.Rotate(Vector3.up, turn * turnSpeed * Time.fixedDeltaTime * 0.75f);

                foreach (TankWheel wheel in leftBeltWheels)
                {
                    if (wheel.wheelCollider.isGrounded)
                    {
                        if (speed < maxSpeed)
                        {
                            if (motor == 0)
                            {
                                wheel.wheelCollider.motorTorque = turn * motorTorque;
                            }
                            else if (turn == 0)
                            {
                                wheel.wheelCollider.motorTorque = motor;
                            }
                            else if (turn < 0)
                            {
                                wheel.wheelCollider.motorTorque = 0;
                            }
                            else if (turn > 0)
                            {
                                wheel.wheelCollider.motorTorque = motor;
                            }
                        }
                        else
                            wheel.wheelCollider.motorTorque = 0;

                        if ((brake < 0 && vectorSpeed > 5) || (brake > 0 && vectorSpeed < -5))
                        {
                            rb.AddForce(transform.forward * brake * 10, ForceMode.Force);
                            brake = Mathf.Abs(brake);
                            wheel.wheelCollider.brakeTorque = brake;
                        }
                        else
                        {
                            wheel.wheelCollider.brakeTorque = 0;
                        }
                    }

                    ApplyLocalPositionToVisuals(wheel);
                }

                foreach (TankWheel wheel in rightBeltWheels)
                {
                    if (wheel.wheelCollider.isGrounded)
                    {
                        if (speed < maxSpeed)
                        {
                            if (motor == 0)
                            {
                                wheel.wheelCollider.motorTorque = -turn * motorTorque;
                            }
                            else if (turn == 0)
                            {
                                if (speed < maxSpeed)
                                    wheel.wheelCollider.motorTorque = motor;
                            }
                            else if (turn < 0)
                            { 
                                if (speed < maxSpeed)
                                    wheel.wheelCollider.motorTorque = motor;
                            }
                            else if (turn > 0)
                            {
                                wheel.wheelCollider.motorTorque = 0;
                            }
                        }
                        else
                            wheel.wheelCollider.motorTorque = 0;

                        if ((brake < 0 && vectorSpeed > 5) || (brake > 0 && vectorSpeed < -5))
                        {
                            rb.AddForce(transform.forward * brake * 10, ForceMode.Force);
                            brake = Mathf.Abs(brake);
                            wheel.wheelCollider.brakeTorque = brake;
                        }
                        else
                        {
                            wheel.wheelCollider.brakeTorque = 0;
                        }
                    }

                    ApplyLocalPositionToVisuals(wheel);
                }

                foreach (CosmeticWheel cosmeticWheel in cosmeticWheels)
                {
                    cosmeticWheel.wheel.Rotate(leftBeltWheels[0].wheelCollider.rpm * 5.75f * Time.fixedDeltaTime, 0, 0);
                }
            }
        }

        void GetOutOfTank()
        {
            foreach (TankWheel tankWheel in rightBeltWheels)
            {
                tankWheel.wheelCollider.motorTorque = 0;
                tankWheel.wheelCollider.brakeTorque = brakePower;
            }

            foreach (TankWheel tankWheel in leftBeltWheels)
            {
                tankWheel.wheelCollider.motorTorque = 0;
                tankWheel.wheelCollider.brakeTorque = brakePower;
            }

            tankMovingSource.Stop();
            startedEngine = false;
        }

        public void ApplyLocalPositionToVisuals(TankWheel tankWheel)
        {
            if (tankWheel.wheelCollider.transform.childCount == 0)
            {
                return;
            }

            Transform visualWheel = tankWheel.wheelCollider.transform.GetChild(0);

            tankWheel.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
            tankWheel.beltBone.transform.position = position;
            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }


        void Sound()
        {
            if (IsInteracting)
            {
                if (!startedEngine)
                {
                    tankMovingSource.Play();
                    startedEngine = true;
                }
                else
                {
                    var audioPitch = vertical - Mathf.Abs(horizontal);

                    tankMovingSource.pitch = audioPitch.Remap(audioParams.min, audioParams.max, audioParams.newMin, audioParams.newMax);
                }
            }
        }
    }
}