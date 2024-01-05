using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Rider_Carrier))]
    [RequireComponent(typeof(InputCollector))]
    public class CarMovement : PolyMono
    {
        [System.Serializable]
        public class Axle
        {
            // Left WheelCollider of axle 
            public WheelCollider leftWheel;
            // Right WheelCollider of axle 
            public WheelCollider rightWheel;

            // If true this axle will be powered by engine
            public bool motor;
            // If true this axle will be capable of steering
            public bool steering;
            // If true this axle will have brakes
            public bool brake;
            // If true this axle will be conected to handbrake
            public bool handBrake;
        }
        [System.Serializable]
        public class Lights
        {
            // List of front lights
            public List<Light> frontLights;
            // Renderer of front lights model
            public Renderer frontLightsRenderer;
            // Material that will be on the lights renderer when are front lights on
            public Material frontLightsOnMaterial;
            // Material that will be on the lights renderer when are front lights off
            public Material frontLightsOffMaterial;
            [Space(10)]
            // List of rear lights
            public List<Light> backLights;
            // Renderer of rear lights model
            public Renderer backLightsRenderer;
            // Material that will be on the lights renderer when are rear lights on
            public Material backLightsOnMaterial;
            // Material that will be on the lights renderer when are rear lights off
            public Material backLightsOffMaterial;
        }
        public override string __Usage => "Car.";

        // Tourqe of motor in Nm
        [Range(100f, 1000f)]
        public float motorTorque = 100;
        // Brake power Nm
        [Range(100f, 1000f)]
        public float brakePower = 100;
        // Maximum speed in km/h that car can reach
        [Range(10f, 300f)]
        public float maxSpeed = 100;
        // Maximum angle of cars turning wheels
        [Range(0f, 90f)]
        public float maxSteeringAngle = 30;
        // List of car axles
        public List<Axle> axles;
        // Front and rear lights
        public Lights lights;
        // Center of mass offseted from center
        public Vector3 centerOfMass;
        // Curve that limits steering angle dependent on speed (where 1 is maxspeed)
        public AnimationCurve steeringCurve;

        private Rigidbody rb;
        bool lightsOn = false;
        //bool interactingWith;
        InputCollector input;

        void Awake()
        {
            var riderLibrary = GetComponent<Rider_Carrier>();
            //riderLibrary.RegisterRiderEnterCallback(r=>interactingWith = true);
            riderLibrary.RegisterRiderEnterCallback(r=>
            {
                if (!riderLibrary.Riders.Any())
                    StopInteracting();
            });
            input = GetComponent<InputCollector>();
            input[Inputs.Reload].OnActivate += HandleChangeLights;
        }

        void HandleChangeLights()
        {
            foreach (Light light in lights.frontLights)
            {
                light.enabled = !lightsOn;
            }
            lightsOn = !lightsOn;
            if (lightsOn)
                lights.frontLightsRenderer.material = lights.frontLightsOnMaterial;
            else
                lights.frontLightsRenderer.material = lights.frontLightsOffMaterial;
        }

        void Start()
        {
            // Set rigidbody and turn off lights
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = centerOfMass;
            foreach (Light light in lights.backLights)
            {
                light.enabled = false;
            }
            foreach (Light light in lights.frontLights)
            {
                light.enabled = false;
            }
        }

        // Applies rotation and position of WheelCollider to the visual model witch must be set as first child of WheelCollider
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
        public void FixedUpdate()
        {
            // Checks if player is driving the car
            //if (interactingWith)
            {
                // Sets variables accordingly to axis input
                var localInput = input[Inputs.Tilt].DirectVector;
                float motor = motorTorque * localInput.y;
                float brake = 0f;//brakePower * localInput.z;
                float steering = maxSteeringAngle * localInput.x;
                float speed = rb.velocity.magnitude;
                steering *= steeringCurve.Evaluate(speed / maxSpeed);
                
                if (lightsOn)
                {
                    // This takes care of rear lights when breaking
                    if ((brake < 0 && speed > 3 && axles[0].rightWheel.rpm > 0) || (brake > 0 && speed > 3 && axles[0].rightWheel.rpm < 0))
                    {
                        foreach (Light light in lights.backLights)
                        {
                            light.enabled = true;
                        }
                        lights.backLightsRenderer.material = lights.backLightsOnMaterial;
                    }
                    else
                    {
                        foreach (Light light in lights.backLights)
                        {
                            light.enabled = false;
                        }
                        lights.backLightsRenderer.material = lights.backLightsOffMaterial;
                    }

                }
                // Updates all wheels
                foreach (Axle axle in axles)
                {
                    // Debug.Log("RMP :" + axle.leftWheel.rpm + " Motor :" + motor + " Brake :" + brake);
                    // Add torque to the powered axles if player commad to
                    if (axle.motor)
                    {
                        if (speed <= maxSpeed)
                        {
                            axle.rightWheel.motorTorque = motor;
                            axle.leftWheel.motorTorque = motor;
                        }
                        else
                        {
                            axle.rightWheel.motorTorque = 10;
                            axle.leftWheel.motorTorque = 10;
                        }
                    }
                    // Add power to brakes on axle if player commads to
                    /*if (axle.brake)
                    {
                        if ((brake < 0 && (axle.rightWheel.rpm + axle.leftWheel.rpm) * 0.5 > 10) || (brake > 0 && (axle.rightWheel.rpm + axle.leftWheel.rpm) * 0.5 < -10))
                        {
                            if (axle.leftWheel.isGrounded || axle.rightWheel.isGrounded)
                                rb.AddForce(transform.GetChild(0).forward * brake * 10, ForceMode.Force);
                            brake = Mathf.Abs(brake);
                            axle.rightWheel.brakeTorque = brake;
                            axle.leftWheel.brakeTorque = brake;

                        }
                        else
                        {
                            axle.rightWheel.brakeTorque = 0;
                            axle.leftWheel.brakeTorque = 0;
                        }
                    }*/
                    // Changes angle of steering wheels
                    if (axle.steering)
                    {
                        axle.rightWheel.steerAngle = steering;
                        axle.leftWheel.steerAngle = steering;
                    }

                    if (axle.handBrake)
                    {
                        if (input[Inputs.Jump].Value)
                        {
                            axle.leftWheel.brakeTorque = brakePower;
                            axle.rightWheel.brakeTorque = brakePower;
                        }
                        else
                        {
                            axle.leftWheel.brakeTorque = 0f;
                            axle.rightWheel.brakeTorque = 0f;
                        }
                    }

                    // Applies changes to bouth wheels on axle
                    ApplyLocalPositionToVisuals(axle.rightWheel);
                    ApplyLocalPositionToVisuals(axle.leftWheel);
                }
            }
            // else
            // {
            //     // Applies changes to visual wheels if player is not controling the car
            //     foreach (Axle axle in axles)
            //     {
            //         ApplyLocalPositionToVisuals(axle.rightWheel);
            //         ApplyLocalPositionToVisuals(axle.leftWheel);
            //     }
            // }
        }
        // Takes care of player exiting car
        public void StopInteracting()
        {
            // Stars breaking the car when player exits
            axles[1].rightWheel.brakeTorque = brakePower;
            axles[1].leftWheel.brakeTorque = brakePower;
            foreach (Axle axles in axles)
            {
                axles.leftWheel.motorTorque = 0;
                axles.rightWheel.motorTorque = 0;
            }

            //interactingWith = true;
        }
    }
}