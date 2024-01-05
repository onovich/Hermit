using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public class Respawner : PolyMono
    {
        public override string __Usage => $"Handles respawning. Things like {nameof(Health_Reservoir)} will automatically register with this.";
        [SerializeField] UnityEvent OnRespawn = default;
        public void RegisterRespawnCallback(UnityAction act) => OnRespawn.AddListener(act);

        public void UnregisterRespawnCallback(UnityAction act) => OnRespawn.RemoveListener(act);

        public void Respawn() => OnRespawn.Invoke();
    }
}