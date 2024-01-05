using System;
using Polyperfect.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Image))]
    public class FillByFraction : PolyMono
    {
        public override string __Usage => $"Sets the Image fill amount based on the {nameof(IFractionalAmount)} source, such as a {nameof(Reservoir)}.";
        [Tooltip("Must be a type that implements IFractionalAmount")][HighlightNull] [SerializeField] MonoBehaviour Source;
        IFractionalAmount SourceAsFraction => (IFractionalAmount) Source;
        
        Image image;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void OnValidate()
        {
            if (!Source||Source is IFractionalAmount)
                return;
            
            Debug.LogWarning($"Provided script must implement {nameof(IFractionalAmount)}, {Source.name} does not. Clearing.");
            Source = null;
        }

        void Update() => UpdateFill();

        void UpdateFill() => image.fillAmount = SourceAsFraction.FractionalAmount;
    }
}