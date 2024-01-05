using System.Collections;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Respawner))]
    [RequireComponent(typeof(Health_Reservoir))]
    
    public class AutoRespawn : PolyMono
    {
        public override string __Usage => "On death, waits the specified amount of time then respawns automatically.";
        [SerializeField] float AutoRespawnDelay = 3f;
        Health_Reservoir health;
        void Start()
        {
            health = GetComponent<Health_Reservoir>();
            health.RegisterDeathCallback(HandleAutoRespawn);
            health.RegisterReviveCallback(StopAllCoroutines);
        }
        

        void HandleAutoRespawn()
        {
            StartCoroutine(DelayRevive());
        }

        IEnumerator DelayRevive()
        {
            yield return new WaitForSeconds(AutoRespawnDelay);
            GetComponent<Respawner>().Respawn();
        }
    }
}