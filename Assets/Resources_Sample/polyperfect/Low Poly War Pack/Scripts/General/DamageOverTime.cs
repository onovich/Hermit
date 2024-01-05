using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Health_Reservoir))]
    public class DamageOverTime : MonoBehaviour
    {
        Health_Reservoir health;
        public float DPS = 5;


        void Awake() => health = GetComponent<Health_Reservoir>();
        void Update() => health.Health -= DPS * Time.deltaTime;
    }
}