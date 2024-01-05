using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(InputCollector))]
    [DisallowMultipleComponent]
    public class CopyInputFromUser_UseEffect : UseEffect
    {
        InputCollector input;
        InputCollector source;
        [HighlightNull] [SerializeField] UsableCapability CancelWhen;

        public override string __Usage => $"Copies the values in an external {nameof(InputCollector)} to the attached one. Registers with {nameof(Capability)}, and unregisters on {nameof(CancelWhen)}.";

        protected override void Initialize()
        {
            input = GetComponent<InputCollector>();
            usable.RegisterUseCallback(act: u =>
            {
                if (u.Capability==CancelWhen)
                    source = null;
            });
        }


        public override void OnUse(UseContext context) => source = context.User.GetComponent<InputCollector>();

        void Update()
        {
            if (source) //onenable and ondisable are used for registering and unregistering, so we need to use a check instead
            {
                input.CopyFrom(source);
            }
        }
    }
}