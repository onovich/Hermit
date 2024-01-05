using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SoldierMovementFromInputs))]
    [RequireComponent(typeof(DynamicUsableHolder))]
    
    public class SoldierAnimationManager : PolyMono
    {
        public override string __Usage => "Specifically handles animations for the builtin soldiers.";
        public string DeathBool = "Death";
        public string RunningBool = "isRunning";
        public string SideSpeed = "Horizontal";
        public string ForwardSpeed = "Vertical";
        public string Jump = "Jump";
        public string RunSpeed = "RunningSpeed";
        public string WalkSpeed = "Speed";
        public string Reloading = "Reloading";

        public string Shoot = "Fire";
        public float WeightBlendSpeed = 2f;
        Animator animator;
        SoldierMovementFromInputs movement;
        float currentHandsWeight = 0f;
        DynamicUsableHolder usableHolder;

        bool IsDoingSomethingWithHands => usableHolder.Usable&&(!movement.IsRunning||animator.GetBool(Reloading));
        void Awake()
        {
            GetComponent<InputCollector>();
            animator = GetComponentInChildren<Animator>();
            movement = GetComponent<SoldierMovementFromInputs>();
            usableHolder = GetComponent<DynamicUsableHolder>();
            usableHolder.RegisterChangeCallback(HandleWeaponChange);
            usableHolder.RegisterUseCallback(HandleWeaponShoot);
            movement.RegisterJumpCallback(HandleJump);
            if (!animator)
            {
                Debug.LogError($"SoldierAnimationManager requires an Animator to be on a child of {name}");
                enabled = false;
            }

            animator.applyRootMotion = false;
        }

        void HandleJump()
        {
            animator.SetTrigger(Jump);
        }

        void HandleWeaponShoot(Usable act)
        {
            animator.SetTrigger(Shoot);
        }

        void HandleWeaponChange(ChangeEvent<Usable> obj)
        {
            if (obj.Previous)
                animator.SetBool(obj.Previous.Name,false);
            if (obj.Next)
                animator.SetBool(obj.Next.Name,true);
        }

        void Start()
        {
            var health = GetComponent<Health_Reservoir>();
            if (health)
            {
                health.RegisterDeathCallback(() => { animator.SetBool(DeathBool,true); });
                health.RegisterReviveCallback(()=>{ animator.SetBool(DeathBool,false);});
            }
        }

        void Update()
        {
            var localVel = movement.LocalVelocity;
            animator.SetBool(RunningBool,movement.IsRunning);
            animator.SetFloat(RunSpeed, 1f);
            animator.SetFloat(WalkSpeed, 1f);
            animator.SetFloat(ForwardSpeed,localVel.z*1.5f);
            animator.SetFloat(SideSpeed,localVel.x*1.5f);
            
            var targetWeight = IsDoingSomethingWithHands ? 1f : 0f;
            currentHandsWeight = Mathf.MoveTowards(currentHandsWeight, targetWeight, WeightBlendSpeed * Time.deltaTime);
            animator.SetLayerWeight(2,currentHandsWeight);
        }
    }
}