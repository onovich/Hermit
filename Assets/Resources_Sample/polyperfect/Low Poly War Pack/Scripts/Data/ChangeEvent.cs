using JetBrains.Annotations;

namespace Polyperfect.War
{
    public readonly struct ChangeEvent<T>
    {
        [CanBeNull]public readonly T Previous;
        [CanBeNull]public readonly T Next;

        public ChangeEvent(T previous, T next)
        {
            Previous = previous;
            Next = next;
        }
    }
}