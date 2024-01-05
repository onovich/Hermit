namespace Polyperfect.War
{
    public readonly struct TrackerTargetPair
    {
        public readonly Tracker Tracker;
        public readonly ITargetable Target;

        public TrackerTargetPair(Tracker tracker, ITargetable target)
        {
            Tracker = tracker;
            Target = target;
        }

        public override int GetHashCode()
        {
            return Tracker.GetHashCode() ^ Target.GetHashCode();
        }
    }
}