using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Respawner))]
    [RequireComponent(typeof(Health_Reservoir))]
    public class FadeOnRespawn : PolyMono
    {
        public override string __Usage => $"Fades out the screen when the character dies, and back after they respawn.";
        [HighlightNull] public GraphicFader Fader;
        void Start()
        {
            var health = GetComponent<Health_Reservoir>();
            var respawner = GetComponent<Respawner>();
            if (!Fader)
            {
                Debug.LogError($"No {nameof(GraphicFader)} found in scene.");
                return;
            }

            health.RegisterDeathCallback(HandleDeath);
            respawner.RegisterRespawnCallback(HandleRespawn);
        }

        void HandleRespawn()
        {
            Fader.Fadeout(1f, () => { });
        }

        void HandleDeath()
        {
            Fader.FadeTo(Color.black,1f, () => { });
        }
    }
}