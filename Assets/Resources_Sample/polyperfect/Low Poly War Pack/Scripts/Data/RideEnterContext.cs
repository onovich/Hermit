namespace Polyperfect.War
{
    [System.Serializable]
    public class RideEnterContext
    {
        public IRideable Rideable;
        public bool DisableColliders;
        public bool DisableRenderers;

        public RideEnterContext(IRideable rideable, bool disableColliders, bool disableRenderers)
        {
            Rideable = rideable;
            DisableColliders = disableColliders;
            DisableRenderers = disableRenderers;
        }
    }
}