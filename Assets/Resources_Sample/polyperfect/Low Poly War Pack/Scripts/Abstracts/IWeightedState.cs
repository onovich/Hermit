namespace Polyperfect.War
{
    public interface IWeightedState
    {
        float GetWeight();
        void ActiveUpdate();
    }
}