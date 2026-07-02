using PolarityBreach.PolaritySystem.Interfaces;
using UnityEngine;

namespace PolarityBreach.PolaritySystem
{
    public static class DamageSystem
    {
        public static bool DealsDamage(Polarity attacker, Polarity target)
        {
            return attacker != target;
        }

        public static bool TryApplyDamage(IPolarizable source, GameObject target, float amount)
        {
            if (source == null || target == null) return false;

            IPolarizable targetPolarity = target.GetComponent<IPolarizable>();
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (targetPolarity == null || damageable == null) return false;

            return TryApplyDamage(source.CurrentPolarity, targetPolarity, damageable, amount);
        }

        public static bool TryApplyDamage(Polarity sourcePolarity, IPolarizable targetPolarity,
            IDamageable damageable, float amount)
        {
            if (targetPolarity == null || damageable == null) return false;
            if (!DealsDamage(sourcePolarity, targetPolarity.CurrentPolarity))
                return false;

            damageable.TakeDamage(amount);
            return true;
        }
    }
}