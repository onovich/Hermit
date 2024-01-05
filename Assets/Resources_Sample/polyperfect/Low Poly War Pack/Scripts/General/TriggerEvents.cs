using Polyperfect.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Polyperfect.War
{
    public class TriggerEvents : PolyMono
    {
        
        public override string __Usage => "Simple callbacks for triggers.";
        public LayerMask Mask = ~0;

        public UnityEvent<Collider> Enter, Stay, Exit;

        void OnTriggerEnter(Collider other)
        {
            if (IsInLayer(other))
                Enter.Invoke(other);
        }

        void OnTriggerStay(Collider other)
        {
            if (IsInLayer(other))
                Stay.Invoke(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (IsInLayer(other))
                Exit.Invoke(other);
        }

        bool IsInLayer(Collider col) => Mask == (Mask | (1 << col.gameObject.layer));
    }
}