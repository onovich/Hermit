namespace Polyperfect.War
{
    public interface IFuzzyState:IWeightedState
    {
        bool TryActivate();
        bool TryDeactivate();
    }

    public static class FuzzyStateExtensions
    {
        public static bool SetActiveState(this IFuzzyState that, bool enable)
        {
            return enable ? that.TryActivate() : that.TryDeactivate();
        }
    }
    
}