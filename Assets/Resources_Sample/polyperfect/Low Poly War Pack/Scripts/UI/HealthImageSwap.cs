using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Image))]
    public class HealthImageSwap : ChangeableWatcher<Health_Reservoir,float>
    {
        public override string __Usage => $"Retrieves {nameof(Health_Reservoir)} values from a parent or override and selects a UI {nameof(Image)} based on the thresholds provided.";
        Image image;
        public List<Sprite> Sprites = new List<Sprite>();
        [Delayed] [Range(0f,1f)] public List<float> FractionValueSeparations = new List<float>();

        void OnValidate()
        {
            for (var i = 0; i < FractionValueSeparations.Count; i++)
                FractionValueSeparations[i] = Mathf.Clamp01(FractionValueSeparations[i]);
            FractionValueSeparations = FractionValueSeparations.OrderBy(f=>f).ToList();
            var lastValue = FractionValueSeparations.LastOrDefault();
            while (FractionValueSeparations.Count<Sprites.Count-1) 
                FractionValueSeparations.Add(lastValue = Mathf.Lerp(lastValue,1f,.5f));
            while (FractionValueSeparations.Count>Sprites.Count-1&&FractionValueSeparations.Count>0)
                FractionValueSeparations.RemoveAt(FractionValueSeparations.Count-1);
        }

        protected override void Initialize()
        {
            image = GetComponent<Image>();
        }

        protected override void HandleValueChange(ChangeEvent<float> e)
        {
            image.sprite = GetSpriteForValue(e.Next/target.MaxHealth);
        }

        Sprite GetSpriteForValue(float f)
        {
            if (Sprites.Count <= 0)
                return null;
            for (var i = 0; i < FractionValueSeparations.Count; i++)
            {
                if (f > FractionValueSeparations[i])
                    continue;
                return Sprites[i];
            }

            return Sprites[Sprites.Count - 1];
        }
    }
}