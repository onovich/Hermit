using UnityEngine;
namespace Polyperfect
{
    namespace War
    {
        public interface IDamageable<T>
        {
            void TakeDamage(T damage);
        }
    }
}
