using UnityEngine;
using UnityEngine.UI;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Text))]
    public class HealthText : ChangeableWatcher<Health_Reservoir, float>
    {
        public override string __Usage => $"Retrieves {nameof(Health_Reservoir)} values from a parent or override and formats it for display by the attached UI {nameof(Text)}.";
        [Tooltip("{0}: current health\n{1}: max health")]
        [Multiline] public string Format = "{0:0}/{1:0}";
        Text text;

        protected override void Initialize() => text = GetComponent<Text>();
        
        protected override void HandleValueChange(ChangeEvent<float> e) => text.text = string.Format(Format, target.Health, target.MaxHealth);
    }
}