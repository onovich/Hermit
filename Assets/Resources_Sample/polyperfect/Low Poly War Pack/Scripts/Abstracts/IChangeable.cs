using UnityEngine.Events;

namespace Polyperfect.War
{
    public interface IChangeable
    {
        void RegisterChangeCallback(UnityAction act);
        void UnregisterChangeCallback(UnityAction act);
    }
    public interface IChangeable<T>
    {
        void RegisterChangeCallback(UnityAction<ChangeEvent<T>> onChange);
        void UnregisterChangeCallback(UnityAction<ChangeEvent<T>> onChange);
        ChangeEvent<T> GetInitializingEvent();
    }
}