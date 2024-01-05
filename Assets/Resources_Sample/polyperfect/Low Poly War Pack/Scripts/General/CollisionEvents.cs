using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public class CollisionEvents:PolyMono
    {
        public override string __Usage => "Callbacks for standard collision events for convenience.";
        public UnityEvent<Collision> Enter, Stay, Exit;

        void OnCollisionEnter(Collision other) => Enter.Invoke(other);

        void OnCollisionStay(Collision other) => Stay.Invoke(other);

        void OnCollisionExit(Collision other) => Exit.Invoke(other);
    }
}