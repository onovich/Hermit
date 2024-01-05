using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Polyperfect.War
{
    public static class DamageUtility
    {
        static readonly Collider[] colliderArray = new Collider[1000];

        public static void AoEDamage([CanBeNull] UseContext useContext, Vector3 position, float radius, float damageAmount, float impactMagnitude)
        {
            var receivers = new HashSet<Damage_Receiver>();
            var count = Physics.OverlapSphereNonAlloc(position,radius,colliderArray);
            for (var i = 0; i < count; i++)
            {
                var item = colliderArray[i];
                var colTransform = item.transform;

                var attachedReceiver = item.GetComponentInParent<Damage_Receiver>();
                if (attachedReceiver && !receivers.Contains(attachedReceiver))
                {
                    receivers.Add(attachedReceiver);
                    var impactVector = (attachedReceiver.transform.position - position).normalized * impactMagnitude;
                    var asMeshCollider = item as MeshCollider;
                    var impactPoint = (!asMeshCollider || asMeshCollider.convex)
                        ? Physics.ClosestPoint(position, item, colTransform.position, colTransform.rotation)
                        : position;
                    attachedReceiver.TakeDamage(new DamageContext(damageAmount, impactVector, useContext, impactPoint));
                }
            }
        }
        
    }
}