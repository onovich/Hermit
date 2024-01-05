using System.Linq;
using UnityEngine;

namespace Polyperfect.War
{
    public abstract class ResourceSeekerBase_AI<TrackerType, TargetType,FractionalType> : SeekerBase_AI<TrackerType, TargetType>
        where TrackerType : Tracker<TargetType> where TargetType : TargetableBase where FractionalType: IFractionalAmount
    {
        FractionalType fractionSource;
        [Range(0f,1f)] public float BeginSeekFraction = .8f;
        [Range(0f, 1f)] public float MaxSeekFraction = .3f; 
        protected override void Initialize()
        {
            base.Initialize();
            fractionSource = GetComponent<FractionalType>();
        }
        protected override float CalculateWeight()
        {
            var ret = 0f;
            if (tracker.Tracked.Any())
                ret = Mathf.InverseLerp(BeginSeekFraction, MaxSeekFraction, fractionSource.FractionalAmount);
            
            return ret;
        }
    }
}