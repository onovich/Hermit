using UnityEngine;

namespace Polyperfect.War
{
    public class AnimationEvent_Sender_UseEffect : UseEffect
    {
        [SerializeField] string[] Triggers;
        public override string __Usage => $"Tells the {nameof(AnimationEvent_Receiver)} on the user to set parameters on the linked {nameof(Animator)}.";
        protected override void Initialize() { }


        public override void OnUse(UseContext context)
        {
            if (context.User && context.User.TryGetComponent(out AnimationEvent_Receiver receiver))
            {
                foreach (var item in Triggers)
                {
                    receiver.SetTrigger(item);
                }
            }
        }
    }
}