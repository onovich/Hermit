using UnityEngine;

static class RaycastHelpers
{
    static readonly RaycastHit[] hits = new RaycastHit[100];
    public static bool IsNonDynamicColliderInRange(Ray ray, float distance)
    {
        var hitCount = Physics.RaycastNonAlloc(ray, hits, distance);
        for (var i = 0; i < hitCount; i++)
            if (!hits[i].collider.GetComponentInParent<Rigidbody>() && !hits[i].collider.GetComponentInParent<CharacterController>())
            {
                return true;
            }

        return false;
    }
}