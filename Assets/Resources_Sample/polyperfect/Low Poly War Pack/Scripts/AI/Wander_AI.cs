using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(InputCollector))]
    public class Wander_AI : FuzzyComponent
    {
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Movement;
        public override string __Usage => "Inputs to make the character wander around randomly.";
        [Range(0f, 1f)] public float SpeedMultiplier = .3f;
        public float Distance = 3f;
        public float WaitMinimum = 2f;
        public float WaitMaximum = 5f;
        TiltTowardsPosition_Input moveToTarget;
        Vector3 wanderOrigin;
        protected override float CalculateWeight() => 1f;

        protected override void Initialize()
        {
            moveToTarget = gameObject.AddAndDisableComponent<TiltTowardsPosition_Input>();
            moveToTarget.StopDistance = .3f;
        }

        public override void ActiveUpdate()
        {
        }

        protected override void HandleActivate()
        {
            wanderOrigin = transform.position;
            moveToTarget.enabled = true;
            SetWander();
        }

        protected override void HandleDeactivate()
        {
            moveToTarget.enabled = false;
        }

        void Reset()
        {
            WeightMultiplier = .1f;
        }

        const float ASSUMED_WALK_SPEED = .3f;

        void SetWander()
        {
            var angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            var x = Mathf.Cos(angle);
            var z = Mathf.Sin(angle);
            var randDuration = Random.Range(WaitMinimum, WaitMaximum);
            var nextPoint = wanderOrigin + Distance * new Vector3(x, 0, z);
            var distance = Vector3.Distance(transform.position, nextPoint);
            var distDuration = distance * ASSUMED_WALK_SPEED / SpeedMultiplier;

            moveToTarget.SetTarget(nextPoint, SpeedMultiplier, () => Wait(distDuration + randDuration));
        }

        void Wait(float duration)
        {
            StartCoroutine(WaitCoroutine(duration));
        }

        IEnumerator WaitCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            SetWander();
        }
    }
}