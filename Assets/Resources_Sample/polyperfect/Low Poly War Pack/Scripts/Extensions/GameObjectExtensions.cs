using UnityEngine;

namespace Polyperfect.War
{
    public static class GameObjectExtensions
    {
        public static T AddAndDisableComponent<T>(this GameObject that) where T : MonoBehaviour
        {
            var component = that.AddComponent<T>();
            component.enabled = false;
            return component;
        }

        public static T AddOrGetComponent<T>(this GameObject that) where T : Component
        {
            var ret = that.GetComponent<T>();
            if (!ret)
                ret = that.AddComponent<T>();
            return ret;
        }
    }
}