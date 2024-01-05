using UnityEngine;
using UnityEngine.UI;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : ChangeableWatcher<Health_Reservoir,float>
    {
        public override string __Usage => $"Retrieves {nameof(Health_Reservoir)} values from a parent or override and fills the attached UI {nameof(Image)} accordingly";
        Image image;
        protected override void Initialize() => image = GetComponent<Image>();
        protected override void HandleValueChange(ChangeEvent<float> e) => image.fillAmount = e.Next / target.MaxHealth;
    }
}