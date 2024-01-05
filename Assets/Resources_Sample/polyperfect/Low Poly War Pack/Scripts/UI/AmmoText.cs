using UnityEngine;
using UnityEngine.UI;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Text))]
    public class AmmoText : ChangeableWatcher<AmmoReservoir_Proxy, ActiveAmmoInfo>
    {
        public override string __Usage => $"Retrieves ammo info from an {nameof(AmmoReservoir_Proxy)} from a parent or manually specified and formats it for display with a UI {nameof(Text)}";
        [Tooltip("{0}: rounds in clip\n{1}: clip size")]//"\n{2}: total rounds")]
        [Multiline] public string Format = "{0:0}/{1:0}";
        Text text;

        protected override void Initialize()
        {
            text = GetComponent<Text>();
            
        }

        protected override void HandleValueChange(ChangeEvent<ActiveAmmoInfo> e) =>
            text.text = string.Format(Format, e.Next.ActiveRounds, e.Next.ClipSize); //, e.Next.TotalRounds);

    }
}