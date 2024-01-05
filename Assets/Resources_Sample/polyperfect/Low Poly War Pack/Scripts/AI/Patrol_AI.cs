using System.Collections.Generic;
using UnityEngine;

namespace Polyperfect.War
{
    
    public class Patrol_AI : FuzzyComponent
    {
        public override string __Usage => "Makes the ai patrol between the given points";
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Movement;
        TiltTowardsPosition_Input tilter;
        public float TiltMagnitude = .5f;
        
        public List<Vector3> TargetPositions;
        int targetIndex;
        protected override void Initialize()
        {
            tilter = gameObject.AddComponent<TiltTowardsPosition_Input>();
        }

        protected override float CalculateWeight() => 1f;

        public override void ActiveUpdate()
        {
            
        }

        protected override void HandleActivate()
        {
            tilter.enabled = true;
            NavigateToClosest();
        }
        void SetNextTarget()
        {
            if (TargetPositions.Count <= 0)
            {
                enabled = false;
                return;
            }

            tilter.SetTarget(TargetPositions[++targetIndex % TargetPositions.Count], TiltMagnitude, SetNextTarget);
        }
        void NavigateToClosest()
        {
            var curPos = transform.position;
            var closest = curPos;
            var closestDistance = Mathf.Infinity;
            for (var i = 0; i < TargetPositions.Count; i++)
            {
                var dist = Vector3.Distance(TargetPositions[i], curPos);
                if (dist < closestDistance)
                {
                    targetIndex = i;
                    closest = TargetPositions[i];
                    closestDistance = dist;
                }
            }

            tilter.SetTarget(closest,TiltMagnitude, SetNextTarget);
        }
        protected override void HandleDeactivate()
        {
            tilter.enabled = false;
        }
    }
}