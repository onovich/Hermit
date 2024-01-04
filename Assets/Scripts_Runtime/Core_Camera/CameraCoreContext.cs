using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public class CameraCoreContext {

        public CameraEntity mainCameraEntity;
        public Camera MainCamera => mainCameraEntity.camera;

        public CameraCoreContext() {
            mainCameraEntity = new CameraEntity();
        }

        public void Inject(Camera mainCamera) {
            mainCameraEntity.Inject(mainCamera);
        }

        public Vector2 GetMainCameraPos() {
            return mainCameraEntity.camera.transform.position;
        }

    }

}