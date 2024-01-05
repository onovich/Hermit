using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Rider_Carrier))]
    public class RideExit_UseEffector : UseEffector
    {
        Rider_Carrier riders;
        [HighlightNull] [SerializeField] Transform ExitTransform;
        public override string __Usage => $"Support for exiting riding. The User must have a {nameof(RiderBase)} attached";

        protected override void Initialize() => riders = GetComponent<Rider_Carrier>();
        public static readonly Reason UserIsNotInTheRideable = new Reason();

        public override bool CheckCondition(UseContext context, out Reason reason)
        {
            var ret = riders.HasRider(context.User.GetComponent<RiderBase>());
            reason = ret ? Reason.None : UserIsNotInTheRideable;
            return ret;
        }

        public override void OnUse(UseContext context) => riders.RemoveRider(context.User.GetComponent<RiderBase>(),ExitTransform.position,ExitTransform.rotation);
    }
}