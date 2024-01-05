namespace Polyperfect.War
{
    public class Medkit_Pickup : Pickup
    {
        public float Amount = 100;
        public float Duration= 1;
        public bool IsInstant => Duration < .000001f;
        public override string __Usage => $"Restores health when picked up. Automatically tracked by {nameof(Medkit_Tracker)}s";

        protected override void Initialize() { }
    }
}