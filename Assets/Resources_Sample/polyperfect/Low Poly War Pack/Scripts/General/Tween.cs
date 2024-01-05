using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polyperfect.War
{
    public static class Tween
    {
        public static Coroutine Arbitrary(MonoBehaviour source, Action<float> act, float duration, Action onComplete)
        {
            return source.StartCoroutine(ArbitraryTween(act, duration, onComplete));
        }

        static IEnumerator ArbitraryTween(Action<float> act, float duration, Action onComplete)
        {
            var startTime = Time.time;
            var endTime = startTime + duration;
            while (Time.time < endTime)
            {
                act(Mathf.InverseLerp(startTime, endTime, Time.time));
                yield return null;
            }

            act(1f);
            onComplete();
        }
    }
}