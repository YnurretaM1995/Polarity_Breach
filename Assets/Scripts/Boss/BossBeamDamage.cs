using PolarityBreach.PolaritySystem;
using PolarityBreach.Player;
using UnityEngine;

namespace PolarityBreach.Boss
{
    [RequireComponent(typeof(PolarityComponent))]
    public class BossBeamDamage : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float damageCooldown = 0.5f;
        
        private PolarityComponent _polarity;
        private float _nextDamageTime;

        void Awake()
        {
            _polarity = GetComponent<PolarityComponent>();
        }

        void OnTriggerStay(Collider other)
        {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth == null) return;

            if (Time.time < _nextDamageTime) return;

            bool hit = DamageSystem.TryApplyDamage(_polarity, playerHealth.gameObject, damage);

            if (hit)
            {
                _nextDamageTime = Time.time + damageCooldown;
            }
        }
    }
}
