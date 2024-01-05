using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Respawner))]
    [RequireComponent(typeof(CharacterController))]
    public class RespawnAtStart : PolyMono
    {
        public override string __Usage => "Causes the character to move back to their starting location when respawning.";
        Vector3 startPos;
        void Start()
        {
            startPos = transform.position;
            GetComponent<Respawner>().RegisterRespawnCallback(HandleRespawn);
        }

        void HandleRespawn()
        {
            var controller = GetComponent<CharacterController>();
            controller.enabled = false;
            transform.position = startPos;
            controller.enabled = true;
        }
    }
}