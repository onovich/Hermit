using UnityEngine;

namespace Polyperfect.War
{
    [RequireComponent(typeof(Rider_Carrier))]
    public class RideEnter_UseEffector : UseEffector
    {
        public override string __Usage => $"Allows entities to ride this object. The User must have a {nameof(RiderBase)} attached";

        Rider_Carrier riders;
        public static readonly Reason UserHasNoRiderBase = new Reason(), RiderUnsupported = new Reason();
        protected override void Initialize() => riders = GetComponent<Rider_Carrier>();

        public override bool CheckCondition(UseContext context, out Reason reason)
        {
            if (context.User && context.User.TryGetComponent(out RiderBase rider))
            {
                var supported = riders.SupportsRider(rider);
                reason = supported ? Reason.None : RiderUnsupported;
                return supported;
            }

            reason = UserHasNoRiderBase;
            return false;
        }

        public override void OnUse(UseContext useContext) => riders.AddRider(useContext.User.GetComponent<RiderBase>());
    }
}