using UnityEngine;

namespace Polyperfect.War
{
    public interface ITargetable
    {
        Vector3 Position { get; }
        GameObject gameObject { get; }
    }
}