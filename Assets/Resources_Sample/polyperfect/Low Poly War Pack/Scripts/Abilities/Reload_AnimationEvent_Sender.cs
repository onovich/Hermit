using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Reload_UseEffector))]
    public class Reload_AnimationEvent_Sender:PolyMono
    {
        public override string __Usage => $"Sends a trigger to the User when reloading with the {nameof(Reload_UseEffector)} begins. Requires that the User have a {nameof(AnimationEvent_Receiver)} attached.";
        public string ReloadTriggerName = "Reload";
        public string ReloadingBoolName = "Reloading";
        Reload_UseEffector reloadEffector;
        void Awake()
        {
            if (string.IsNullOrEmpty(ReloadTriggerName))
                return;

            reloadEffector = GetComponent<Reload_UseEffector>();
            
            reloadEffector.RegisterBeginReloadCallback(HandleReloadBegin);
            reloadEffector.RegisterEndReloadCallback(HandleReloadComplete);
            reloadEffector.RegisterCancelReloadCallback(HandleReloadCancel);
        }

        GameObject lastUser;

        void HandleReloadCancel()
        {
            if (!lastUser)
                return;
            var receiver = lastUser.GetComponent<AnimationEvent_Receiver>();
            if (!receiver)
                return;
            receiver.SetFalse(ReloadingBoolName);
            receiver.SetFalse(ReloadTriggerName);
        }

        void HandleReloadBegin(UseContext context)
        {
            lastUser = context.User;
            var receiver = context.User?lastUser.GetComponent<AnimationEvent_Receiver>():null;
            if (!receiver)
                return;
            
            receiver.SetTrigger(ReloadTriggerName);
            receiver.SetTrue(ReloadingBoolName);
        }

        void HandleReloadComplete(UseContext context)
        {            
            var receiver = context.User?context.User.GetComponent<AnimationEvent_Receiver>():null;
            if (!receiver)
                return;
            receiver.SetFalse(ReloadingBoolName);
            receiver.SetFalse(ReloadTriggerName);
        }
    }
}