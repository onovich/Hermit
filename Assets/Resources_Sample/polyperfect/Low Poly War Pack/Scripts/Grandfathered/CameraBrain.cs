using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace Polyperfect
{
    namespace War
    {
        //[ExecuteInEditMode]
        [DefaultExecutionOrder(100)]
        public class CameraBrain : MonoBehaviour
        {
            // Curently using camera
            [SerializeField] VirtualCamera currentCamera;
            // Time (in seconds) that takes transition from one camera to the other
            public float transitionTime;
            // List of active virtual cameras in the scene
            private List<VirtualCamera> virtualCameras = new List<VirtualCamera>();
            // Camera
            private Camera mainCamera;
            ParentConstraint constraint;
            Transform currentTarget;
            public static CameraBrain Instance;
            Vector3 lastFollowPos;
            private void OnEnable()
            {
                Instance = this;
            }

            public void AddCamera(VirtualCamera cam)
            {
                virtualCameras.Add(cam);
            }
            void Awake()
            {
                Instance = this;
                mainCamera = Camera.main;
                int cameraPriority = 0;
                //virtualCameras = VirtualCamera.virtualCameras;
                // Sets the virtual camera with the highest priority as active 
                foreach (VirtualCamera virtualCamera in virtualCameras)
                {
                    if (virtualCamera.priority > cameraPriority)
                    {
                        cameraPriority = virtualCamera.priority;
                        currentCamera = virtualCamera;
                    }
                    virtualCamera.isActive = false;
                }
                //currentCamera.isActive = true;
                if (Application.isPlaying&&currentCamera)
                    SetParent(currentCamera.cameraTransform);
                // Reset Position and rotation
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                constraint = gameObject.AddOrGetComponent<ParentConstraint>();
                constraint.AddSource(new ConstraintSource());
            }
            // This function handles priority changes of virtual cameras
            public void OnPriorityChange()
            {
                if (!this)
                {
                    return;
                }
                
                int cameraPriority = int.MinValue;//currentCamera.priority;
                VirtualCamera previousCam = currentCamera;
                bool change = false;
                // Checks if there is a virtual camera with higher priority than the current one
                foreach (VirtualCamera virtualCamera in virtualCameras.Where(v=>v&&v.enabled))
                {
                    if (virtualCamera.priority > cameraPriority)
                    {
                        cameraPriority = virtualCamera.priority;
                        currentCamera = virtualCamera;
                        change = true;
                    }
                }
                // If there is a change of the highest priority, the transition will be performed
                if (change)
                {
                    // Checks if the camera will inherit the position from previous
                    if (currentCamera&&previousCam&&currentCamera.InheritPosition)
                    {
                        if (previousCam.cameraTransform&&previousCam.followTarget)
                        {
                            currentCamera.lastPos = previousCam.followTarget.position;
                            currentCamera.transform.position = previousCam.transform.position;
                            currentCamera.cameraTransform.position = previousCam.cameraTransform.position;
                        }
                        else
                        {
                            var position = transform.position;
                            currentCamera.lastPos = lastFollowPos;
                            var setPos = previousCam?previousCam.transform.position:position;
                            currentCamera.transform.position = setPos;
                            currentCamera.cameraTransform.position = setPos;
                        }
                    }
                    //transform.parent = null;
                    SetParent(null);
                    if (previousCam)
                        previousCam.isActive = false;
                    currentCamera.isActive = true;
                    if (gameObject&&gameObject.activeInHierarchy)
                    {
                        StopCoroutine("MoveToPosition");
                        StartCoroutine("MoveToPosition", transitionTime);
                    }
                }
            }
            // Performs a transition to new virtual camera
            public IEnumerator MoveToPosition(float timeToMove)
            {
                var t = 0f;
                while (t < 1)
                {
                    if (!currentCamera)
                    {
                        Debug.Log("Breaking");
                        yield break;
                    }
                    t += Time.deltaTime / timeToMove;
                    transform.position = Vector3.Lerp(transform.position, currentCamera.cameraTransform.position, t);
                    transform.rotation = Quaternion.Slerp(transform.rotation, currentCamera.cameraTransform.rotation, t);
                    mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, currentCamera.fOV, t);
                    yield return null;
                }
                SetParent(currentCamera.cameraTransform);
            }

            void LateUpdate()
            {
                if (!currentTarget||!currentCamera)
                {
                    SetParent(null);
                    //OnPriorityChange();
                }

                if (currentCamera)
                    lastFollowPos = currentCamera.followTarget.position;
            }

            void SetParent(Transform trans)
            {
                if (!constraint)
                    return;
                if (currentTarget)
                    currentTarget.gameObject.AddOrGetComponent<SimpleEvents>().OnInactive.RemoveListener(HandleTargetInactive);
                currentTarget = trans;
                if (currentTarget)
                    currentTarget.gameObject.AddOrGetComponent<SimpleEvents>().OnInactive.AddListener(HandleTargetInactive);
                if (trans)
                    constraint.SetSource(0,new ConstraintSource(){sourceTransform = trans,weight = 1f});
                constraint.constraintActive = trans;
            }

            void HandleTargetInactive()
            {
                SetParent(null);
                OnPriorityChange();
            }
        }
        
    }
}
