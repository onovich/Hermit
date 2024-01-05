using System.Collections;
using Polyperfect.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Polyperfect.War
{

    [RequireComponent(typeof(RiderBase))]
    public class ParachuteSpawner : PolyMono
    {
        public override string __Usage => $"Spawns a parachute, or any {nameof(IRideable)}, when the ground is out of the specified range and {nameof(TrySpawnParachute)} is called.";
        public float InitialCheckDistance = 20f;
        [HighlightNull] public GameObject Parachute;
        public float AngleDeviation = 30f;
        RiderBase rider;

        void OnValidate()
        {
            if (!Parachute || Parachute.GetComponent<IRideable>() != null)
                return;
            Debug.LogError($"Parachute object must have an implementer of {nameof(IRideable)} attached, such as a {nameof(Rider_Carrier)}");
            Parachute = null;
        }

        void Start()
        {
            rider = GetComponent<RiderBase>();
            TrySpawnParachute();
        }

        public void TrySpawnParachute()
        {
            StartCoroutine(DoParachuteDelayed());
        }

        IEnumerator DoParachuteDelayed()
        {
            yield return null;
            if (IsGroundInRange(InitialCheckDistance))
            {
                yield break;
            }

            DoParachuteSpawn();
        }

        bool IsGroundInRange(float distance) => RaycastHelpers.IsNonDynamicColliderInRange(new Ray(transform.position-Physics.gravity.normalized*.1f, Physics.gravity.normalized), distance);

        void DoParachuteSpawn()
        {
            var spawnRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * Quaternion.AngleAxis(AngleDeviation, Vector3.forward);
            var instantiated = Instantiate(Parachute, transform.position, spawnRotation);
            instantiated.GetComponent<IRideable>().AddRider(rider);
        }

    }
}