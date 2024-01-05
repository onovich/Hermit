using System.Collections.Generic;
using UnityEngine;

namespace Polyperfect.War
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> AllChildren(this Transform that)
        {
            yield return that;
            foreach (Transform child in that)
            foreach (var nested in child.AllChildren())
                yield return nested;
        }
    }
}