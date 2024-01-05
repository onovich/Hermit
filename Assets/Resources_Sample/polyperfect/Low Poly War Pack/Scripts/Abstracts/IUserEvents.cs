using UnityEngine.Events;

namespace Polyperfect.War
{
    public interface IUserEvents
    {
        void RegisterUseCallback(UnityAction<Usable> act);
        void UnregisterUseCallback(UnityAction<Usable> act);
        void RegisterUseFailureCallback(UnityAction<UseFailedContext> act);
        void UnregisterUseFailureCallback(UnityAction<UseFailedContext> act);
    }
}