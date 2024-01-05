using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(DynamicUsableHolder))]
    [RequireComponent(typeof(InputCollector))]
    [RequireComponent(typeof(Animator_Proxy))]
    public class AnimatorIK_GunAimer : GunAimer
    {
        public override string __Usage =>
            $"Aims the attached gun using the OnAnimatorIK of the character. Requires IK to be enabled in the relevant layer of the {nameof(Animator)}";
        public string AimingParameter = "Aiming";

        public IKWeight IKWeights = new IKWeight(1f, 1f, .2f);
        Animator animator => childIK.Animator;
        Animator_Proxy childIK;
        InputCollector input;
        AxisTurning turning;

        DynamicUsableHolder usableHolder;


        void Awake()
        {
            childIK = GetComponent<Animator_Proxy>();
            usableHolder = GetComponent<DynamicUsableHolder>();
            input = GetComponent<InputCollector>();
            turning = gameObject.AddComponent<AxisTurning>();
            turning.enabled = false;

            
            OnBeginAiming.AddListener(() => turning.enabled = true);
            OnEndAiming.AddListener(() => turning.enabled = false);

            OnBeginAiming.AddListener(() => animator.SetBool(AimingParameter, true));
            OnEndAiming.AddListener(() => animator.SetBool(AimingParameter, false));

            OnEndAiming.AddListener(()=>usableHolder.ClearUsableRotation());

            if (gameObject.TryGetComponent(out Health_Reservoir reservoir))
            {
                reservoir.RegisterDeathCallback(() => enabled = false);
                reservoir.RegisterReviveCallback(()=>enabled = true);
            }
        }

        protected void OnEnable()
        {
            input[Inputs.Aim].OnActivate += HandleBeginAim;
            input[Inputs.Aim].OnDeactivate += HandleEndAim;
        }

        protected void OnDisable()
        {
            HandleEndAim();
            input[Inputs.Aim].OnActivate -= HandleBeginAim;
            input[Inputs.Aim].OnDeactivate -= HandleEndAim;
        }

        protected override void HandleBeginAim()
        {
            base.HandleBeginAim();
            childIK.RegisterIKCallback(HandleAnimatorIK);
        }

        protected override void HandleEndAim()
        {
            base.HandleEndAim();
            childIK.UnregisterIKCallback(HandleAnimatorIK);
        }
        void HandleAnimatorIK(int layerIndex)
        {
            if (activeAimer==default)
            {
                Debug.LogError($"No active {nameof(IAimEffector)} on {gameObject.name}");
                enabled = false;
                return;
            }

            var aimPosition = activeAimer.GetAimPosition();
            turning.TargetDirection = aimPosition - transform.position;
            animator.SetLookAtPosition(aimPosition);

            animator.SetLookAtWeight(1f, IKWeights.Body, IKWeights.Head, IKWeights.Eye);

            usableHolder.AimAt(aimPosition);
            Debug.DrawLine(transform.position,aimPosition,Color.red);
        }

        //public override Vector3 CurrentAimDirection => usableHolder.Usable ? (usableHolder.Usable.transform.rotation*Vector3.forward) : Vector3.forward;
        public override Vector3 CurrentAimPosition =>usableHolder.Usable ? (usableHolder.Usable.transform.rotation*(Vector3.forward*Vector3.Distance(usableHolder.Usable.transform.position,activeAimer.GetAimPosition()))) : Vector3.forward;
    }
}