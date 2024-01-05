namespace Polyperfect.War
{
    public readonly struct UseFailedContext
    {
        public readonly IUseCondition Condition;
        public readonly Reason Reason;

        public UseFailedContext(IUseCondition condition, Reason reason)
        {
            Condition = condition;
            Reason = reason;
        }
    }

    public class Reason
    {
        public static readonly Reason None  = new Reason();
    }
}