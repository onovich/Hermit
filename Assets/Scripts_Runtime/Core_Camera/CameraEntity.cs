using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hermit {

    public class CameraEntity {

        public Camera camera;
        public float zoomValue;

        Vector3 confinerMin;
        Vector3 confinerMax;

        public CameraEntity() {
        }

        public void SetConfiner(Vector3 min, Vector3 max) {
            confinerMin = min;
            confinerMax = max;
        }

        public void Inject(Camera camera) {
            this.camera = camera;
        }

    }

}