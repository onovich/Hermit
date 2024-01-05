using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class AnimationEvent_Sender : PolyMono
    {
        public override string __Usage => $"Contains methods to send events to {nameof(AnimationEvent_Receiver)}s.\nTo use with UnityEvents, first use {nameof(SetParamName)}, then follow it up with {nameof(SendSetTrigger)}.";

        string paramName = "";
        [SerializeField] bool RequireReceiver = true;
        public void SetParamName(string newTriggerName)
        {
            paramName = newTriggerName;
        }

        public void SendSetTrue(UseContext context)
        {
            if (!EnsureReceiver(context, out var receiver)) 
                return;
            receiver.SetTrue(paramName);
        }

        public void SendSetFalse(UseContext context)
        {
            if (!EnsureReceiver(context, out var receiver))
                return;
            receiver.SetFalse(paramName);
        }
        public void SendSetTrigger(UseContext context)
        {
            if (!EnsureReceiver(context, out var receiver))
                return;
            receiver.SetTrigger(paramName);
        }

        bool EnsureReceiver(UseContext context, out AnimationEvent_Receiver receiver)
        {
            var target = context.User;
            receiver = null;
            if (!target)
                return false;

            receiver = target.GetComponent<AnimationEvent_Receiver>();
            if (!receiver)
            {
                if (RequireReceiver)
                    Debug.LogError($"There is no {nameof(AnimationEvent_Receiver)} attached to {target.name}. One is required by {gameObject.name}");
                return false;
            }

            return true;
        }
    }
}