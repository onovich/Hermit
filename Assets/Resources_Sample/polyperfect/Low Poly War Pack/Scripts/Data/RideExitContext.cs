using UnityEngine;

namespace Polyperfect.War
{
    [System.Serializable]
    public class RideExitContext
    {
        public IRideable Rideable;
        public Vector3 SpawnPosition;
        public Quaternion SpawnRotation;

        public RideExitContext(IRideable rideable, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            Rideable = rideable;
            SpawnPosition = spawnPosition;
            SpawnRotation = spawnRotation;
        }
    }
}