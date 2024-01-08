#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hermit.Modifier {

    [ExecuteInEditMode]
    public class TileTerrainEditorEntity : MonoBehaviour {

        [SerializeField] TileTerrainSO so;

        MeshFilter mf;
        MeshRenderer mr;

        public void Update() {
            UpdateMeshRenderer();
        }

        void UpdateMeshRenderer() {
            if (so == null) return;
            if (so.tm == null) return;
            if (so.tm.mesh == null) return;
            if (so.tm.materials == null) return;
            if (so.tm.materials.Length == 0) return;
            if (mf == null) mf = transform.GetChild(0).GetComponent<MeshFilter>();
            if (mr == null) mr = transform.GetChild(0).GetComponent<MeshRenderer>();
            mf.mesh = so.tm.mesh;
            mr.materials = so.tm.materials;
        }

    }

}
#endif