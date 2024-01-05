using UnityEngine;

namespace Polyperfect.War
{
    public class RowSpawner : MonoBehaviour
    {
        public GameObject ToInstantiate;
        public int Count = 10;
        public Vector3 Offset = Vector3.right * 3f;
        public bool AutoSpawn = true;

        void Start()
        {
            if (AutoSpawn)
                DoSpawn();
        }

        public void DoSpawn()
        {
            var trans = transform;
            var pos = trans.position;
            var vect = trans.TransformDirection(Offset);
            var rot = trans.rotation;
            for (var i = 0; i < Count; i++)
            {
                Instantiate(ToInstantiate, pos, rot);
                pos += vect;
            }
        }
    }
}