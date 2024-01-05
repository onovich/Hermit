using System;

namespace Polyperfect.War
{
    public interface IUseChecker
    {
        bool CheckConditions(UseContext context, Func<IUseCondition, bool> conditionFilter);
    }
}