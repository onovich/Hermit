using System;
using System.Collections;
using System.Collections.Generic;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicFader : PolyMono
    {
        Graphic graphic;
        public override string __Usage => "Fade UI Graphic when something happens.";
        public float DefaultFadeDuration = .2f;

        void Awake()
        {
            graphic = GetComponent<Graphic>();
        }


        public void Fadeout(float duration)
        {
            Fadeout(duration, () => { graphic.enabled = false; });
        }

        public void Fadeout(float duration, Action onComplete)
        {
            StopAllCoroutines();
            var startColor = graphic.color;
            var endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
            FadeTo(endColor, duration, onComplete);
        }

        public void FadeIn(float duration)
        {
            StopAllCoroutines();
            FadeTo(1f, duration, () => { });
        }

        public void FadeTo(Color targetColor, float duration, Action onComplete)
        {
            graphic.enabled = true;
            var startColor = graphic.color;
            Tween.Arbitrary(this, f => { graphic.color = Color.Lerp(startColor, targetColor, f); }, duration, onComplete);
        }

        public void FadeTo(float targetAlpha, float duration, Action onComplete)
        {
            graphic.enabled = true;
            var startColor = graphic.color;
            Tween.Arbitrary(this, f => { graphic.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, targetAlpha), f); },
                duration, onComplete);
        }

        public void FadeFor(float displayDuration)
        {
            StopAllCoroutines();
            StartCoroutine(FadeForCoroutine(displayDuration));
        }

        IEnumerator FadeForCoroutine(float displayDuration)
        {
            FadeTo(1f, DefaultFadeDuration, () => { });
            yield return new WaitForSeconds(DefaultFadeDuration + displayDuration);
            FadeTo(0f, DefaultFadeDuration, () => { graphic.enabled = false; });
        }
    }
}