using System;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    public class Animator_Proxy : PolyMono
    {
        public override string __Usage => $"Acts as a constant reference for an {nameof(Animator)} on one of the children.";
        AnimatorIKCallback_Forwarder forwarder;
        public Animator Animator { get; protected set; }
        void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            if (!Animator)
            {
                Debug.LogError($"{nameof(Animator_Proxy)} requires a child with an Animator");
                enabled = false;
                return;
            }

            forwarder = Animator.GetComponent<AnimatorIKCallback_Forwarder>();
            if (!forwarder)
                forwarder = Animator.gameObject.AddComponent<AnimatorIKCallback_Forwarder>();
        }
        public void RegisterIKCallback(Action<int> act) => forwarder.RegisterIKCallback(act);

        public void UnregisterIKCallback(Action<int> act) => forwarder.UnregisterIKCallback(act);

        void OnEnable()
        {
            if (Animator)
                Animator.enabled = true;
        }

        void OnDisable()
        {
            if (Animator)
                Animator.enabled = false;
        }
    }
}