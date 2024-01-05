namespace Polyperfect.War
{
    public interface IUseCondition
    {
        bool CheckCondition(UseContext context, out Reason reason);
    }
    
}