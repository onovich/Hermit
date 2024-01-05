using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{
    [DisallowMultipleComponent]
    public class UseContext_Holder : PolyMono,IUseContextReceiver
    {
        public override string __Usage => $"Holder for {nameof(UseContext)} data.\nPropagates to other {nameof(IUseContextReceiver)}s.\nAutomatically added and used by {nameof(Spawner)} components.";
        public UseContext Context { get; private set; }
        public void Receive(UseContext context)
        {
            if (context==null)
                Debug.LogError("Received null context");
            Context = context;
            foreach (var item in GetComponents<IUseContextReceiver>())
            {
                if (!ReferenceEquals(item, this))
                {
                    item.Receive(context);
                }
            }
        }
    }
}