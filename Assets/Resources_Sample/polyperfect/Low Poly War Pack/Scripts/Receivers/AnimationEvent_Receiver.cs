using UnityEngine;

namespace Polyperfect.War
{
    public class AnimationEvent_Receiver : MonoBehaviour
    {
        Animator animator;
        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            if (!animator)
            {
                Debug.LogError($"No animator on child of {gameObject.name}");
                enabled = false;
            }
        }
        public void SetTrigger(string trigger)
        {
            if (!enabled)
                return;
            
            animator.SetTrigger(trigger);
        }

        public void SetBool(string parameter, bool value) => animator.SetBool(parameter,value);

        public void SetTrue(string parameter) => SetBool(parameter,true);
        public void SetFalse(string parameter) => SetBool(parameter, false);
    }
}