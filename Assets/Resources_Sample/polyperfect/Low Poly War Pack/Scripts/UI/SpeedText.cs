using Polyperfect.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Text))]
    public class SpeedText : PolyMono
    {
        public override string __Usage => "Displays the speed of a parent's Rigidbody on the attached Text element. By default, in meters per second.";
        public string Format = "{0.} m/s";
        public float ValueMutliplier = 1f;
        Rigidbody body;
        Text text;

        void Start()
        {
            body = GetComponentInParent<Rigidbody>();
            text = GetComponent<Text>();
            if (!body)
            {
                Debug.LogError($"Parent of {gameObject.name} must have a rigidbody to be used with {nameof(SpeedText)}.");
                enabled = false;
            }
        }

        void Update()
        {
            text.text = string.Format(Format,body.velocity.magnitude*ValueMutliplier);
        }
    }
}