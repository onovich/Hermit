namespace Polyperfect.War
{
    public interface IExtractable<T>
    {
        T ExtractPossible(T maxAmount);
        bool CanFullyExtract(T amount);
    }
}